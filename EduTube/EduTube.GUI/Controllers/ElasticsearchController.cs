using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace EduTube.GUI.Controllers
{
    public class ElasticsearchController : Controller
    {
        private IVideoManager _videoManager;
        private IApplicationUserManager _applicationUserManager;
        private IHashtagRelationshipManager _hashtagRelationshipManager;

        public ElasticsearchController(IVideoManager videoManager, IApplicationUserManager applicationUserManager,
            IHashtagRelationshipManager hashtagRelationshipManager)
        {
            _videoManager = videoManager;
            _applicationUserManager = applicationUserManager;
            _hashtagRelationshipManager = hashtagRelationshipManager;
        }

        public IActionResult Search(string search)
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("videos");

            ElasticClient client = new ElasticClient(settings);

            ISearchResponse<VideoModel> videoResponse = client.Search<VideoModel>(s => s
                .Index("videos")
                .AllTypes()
                .From(0)
                .Size(50)
                .Query(q => q
                    .MatchPhrasePrefix(m => m
                        .Field(f => f.DateCreatedOn)
                        .Query(search)
                    )
                    || 
                    q.MatchPhrasePrefix(m => m
                        .Field(f => f.Description)
                        .Query(search)
                    )
                    ||
                    q.MatchPhrasePrefix(m => m
                        .Field(f => f.Name)
                        .Query(search)
                    )
                    ||
                    q.MatchPhrasePrefix(m => m
                        .Field(f => f.User.ChannelName)
                        .Query(search)
                    )
                )
            );
            ISearchResponse<ApplicationUserModel> usersResponse = client.Search<ApplicationUserModel>(s => s
                .Index("users")
                .AllTypes()
                .From(0)
                .Size(50)
                .Query(q => q
                    .MatchPhrasePrefix(m => m
                        .Field(f => f.Firstname)
                        .Query(search)
                    )
                    ||
                    q.MatchPhrasePrefix(m => m
                        .Field(f => f.Lastname)
                        .Query(search)
                    )
                    ||
                    q.MatchPhrasePrefix(m => m
                        .Field(f => f.ChannelName)
                        .Query(search)
                    )
                )
            );


            return Json((videoResponse, usersResponse));
        }

        public async Task<IActionResult> GetVideosIndex()
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("videos");

            ElasticClient client = new ElasticClient(settings);

            ISearchResponse<VideoModel> searchResponse = client.Search<VideoModel>(s => s
                 .AllTypes()
                 .From(0)
                 .Size(10)
                 .Query(q => q
                    .MatchPhrasePrefix(m => m
                        .Field(f => f.Name)
                        .Query("")
                    )
                    ||
                    q.MatchPhrasePrefix(m => m
                        .Field(f => f.Description)
                        .Query("")
                    )
                )
            );
            IReadOnlyCollection<VideoModel> videos = searchResponse.Documents;
            if (videos.Count == 0)
            {
               await IndexFromDb();
            }
            List<VideoModel> datasend = (from hits in searchResponse.Hits
                            select hits.Source).OrderBy(e => e.Name).ToList();
            return Json(datasend);
        }

        public IActionResult IndexVideo(VideoModel video)
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("videos");

            ElasticLowLevelClient lowlevelClient = new ElasticLowLevelClient(settings);

            List<Object> listObj = new List<object>();

            Object indexObj = new { index = new { _index = "videos", _type = "videomodel", _id = video.Id } };
            Object videoObj = new
            {
                id = video.Id.ToString(),
                name = video.Name,
                description = video.Description.ToString(),
                channelName = video.User.ChannelName,
                dateCreatedOn = video.DateCreatedOn.ToString("yyyy-MM-dd")
            };

            listObj.Add(indexObj);
            listObj.Add(videoObj);

            StringResponse indexResponseList = lowlevelClient.Bulk<StringResponse>(PostData.MultiJson(listObj));
            string responseStream = indexResponseList.Body;

            return Json(responseStream);
        }

        public async Task<IActionResult> IndexFromDb()
        {

            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("videos");

            ElasticLowLevelClient lowlevelClient = new ElasticLowLevelClient(settings);

            List<VideoModel> videos = await _videoManager.GetAll();
            List<ApplicationUserModel> users = await _applicationUserManager.GetAll();

            List<Object> videosObj = new List<object>();
            List<Object> usersObj = new List<object>();

            foreach (VideoModel video in videos)
            {
                video.User = await _applicationUserManager.GetById(video.UserId, false);
                video.Hashtags = await _hashtagRelationshipManager.GetByVideoId(video.Id, true);
                string hashtagNme = string.Join(",", video.Hashtags.Select(x => x.Hashtag).Select(x => x.Name));

                Object indexObj = new { index = new { _index = "videos", _type = "videomodel", _id = video.Id } };
                Object videoObj = new
                {
                    id = video.Id,
                    name = video.Name,
                    description = video.Description,
                    channelName = video.User?.ChannelName,
                    hashtagsName = hashtagNme,
                    dateCreatedOn = video.DateCreatedOn.ToString("yyyy-MM-dd")
                };

                videosObj.Add(indexObj);
                videosObj.Add(videoObj);
            }

            foreach (ApplicationUserModel user in users)
            {
                Object indexObj = new { index = new { _index = "users", _type = "applicationusermodel", _id = user.Id.ToString() } };
                Object userObj = new
                {
                    id = user.Id,
                    dateOfBirth = user.DateOfBirth.ToString("yyyy-MM-dd"),
                    firstName = user.Firstname,
                    lastName = user.Lastname,
                    channelName = user.ChannelName,
                    profileImage = user.ProfileImage
                };

                usersObj.Add(indexObj);
                usersObj.Add(userObj);
            }

            StringResponse vidoesResponse = lowlevelClient.Bulk<StringResponse>(PostData.MultiJson(videosObj));
            StringResponse usersResponse = lowlevelClient.Bulk<StringResponse>(PostData.MultiJson(usersObj));
            string videosResponseStream = vidoesResponse.Body;
            string usersResponseStream = usersResponse.Body;

            return Json((videosResponseStream, usersResponseStream));
        }

        public IActionResult CreateIndex()
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("videos");

            ElasticClient client = new ElasticClient(settings);

            ICreateIndexResponse videoIndex = client.CreateIndex("videos", c => c
                 .Settings(s => s
                      .Analysis(a => a
                            .Analyzers(an => an
                                 .Standard("standard_english", sa => sa 
                                    .StopWords("_english_")
                                )
                            )
                      )
                 )
                 .Mappings(m => m
                      .Map<VideoModel>(vm => vm
                            .Properties(p => p
                                .Text(t => t
                                    .Name(n => n
                                    .Id)
                                ).Date(t => t
                                      .Name(n => n.DateCreatedOn)
                                 ).Text(t => t
                                      .Name(n => n.Description)
                                      .Analyzer("standard_english")
                                 ).Text(t => t
                                      .Name(n => n.Name)
                                      .Analyzer("standard_english")
                                 ).Text(t => t
                                      .Name(n => n.User.ChannelName)
                                      .Analyzer("standard_english")
                                 ).Text(t => t
                                    .Name("hashtagsName")
                                    .Analyzer("standard_english")
                                 )
                            )
                      )
                 )
            );

            ICreateIndexResponse usersIndex = client.CreateIndex("users", c => c
                 .Settings(s => s
                      .Analysis(a => a
                            .Analyzers(an => an
                                 .Standard("standard_english", sa => sa
                                    .StopWords("_english_")
                                )
                            )
                      )
                 )
                 .Mappings(m => m
                      .Map<ApplicationUserModel>(vm => vm
                            .Properties(p => p
                                .Text(t => t
                                    .Name(n => n
                                    .Id)
                                 ).Date(t => t
                                      .Name(n => n.DateOfBirth)
                                 ).Text(t => t
                                      .Name(n => n.Firstname)
                                      .Analyzer("standard_english")
                                 ).Text(t => t
                                      .Name(n => n.Lastname)
                                      .Analyzer("standard_english")
                                 ).Text(t => t
                                      .Name(n => n.ChannelName)
                                      .Analyzer("standard_english")
                                 ).Text(t => t
                                      .Name(n => n.ProfileImage)
                                      .Analyzer("standard_english")
                                 )
                            )
                      )
                 )
            );
            return Json((videoIndex, usersIndex));
        }

        // GET: Elasticsearch
        public IActionResult Index()
        {
            return View();
        }
    }
}