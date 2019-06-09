using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.GUI.ViewModels;
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

        [Route("Search")]
        public async Task<IActionResult> Search(string search_query, string search_etity, int? page)
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"));
            ElasticClient client = new ElasticClient(settings);

            if (page != null)
                page = (page-1) * 10;
            else
                page = 0;
            ISearchResponse<VideoModel> videosResponse = null;
            ISearchResponse<ApplicationUserModel> usersResponse = null;
            #region videosSearch
            if (search_etity == null || search_etity.Equals("videos"))
            {
                videosResponse = await client.SearchAsync<VideoModel>(s => s
                    .Index("videos")
                    .AllTypes()
                    .From(page)
                    .Size(10)
                    .Query(q =>
                        q.Fuzzy(f => f
                            .Field(fi => fi.Name)
                            .Fuzziness(Fuzziness.EditDistance(2))
                            .PrefixLength(0)
                            .Transpositions(true)
                            .MaxExpansions(100)
                            .Value(search_query)
                        )
                        ||
                        q.Fuzzy(f => f
                            .Field(fi => fi.Description)
                            .Fuzziness(Fuzziness.EditDistance(2))
                            .PrefixLength(0)
                            .Transpositions(true)
                            .MaxExpansions(100)
                            .Value(search_query)
                        )
                        ||
                        q.Fuzzy(f => f
                            .Field(fi => fi.UserChannelName)
                            .Fuzziness(Fuzziness.EditDistance(2))
                            .PrefixLength(0)
                            .Transpositions(true)
                            .MaxExpansions(100)
                            .Value(search_query)
                        )
                        ||
                        q.Fuzzy(f => f
                            .Field(fi => fi.Hashtags)
                            .Fuzziness(Fuzziness.EditDistance(2))
                            .PrefixLength(0)
                            .Transpositions(true)
                            .MaxExpansions(100)
                            .Value(search_query)
                        )
                    )
                    .Highlight(h => h
                        .PreTags("<strong>")
                        .PostTags("</strong>")
                        .Encoder(HighlighterEncoder.Html)
                        .Fields(fs =>
                            fs.Field(f => f.Name)
                            .Type(HighlighterType.Plain)
                            .PreTags("<strong class='blue'>")
                            .PostTags("</strong>")
                            .ForceSource()
                            .Fragmenter(HighlighterFragmenter.Span)
                            .HighlightQuery(q => q
                                .Fuzzy(m =>
                                    m.Field(fi => fi.Name)
                                     .Fuzziness(Fuzziness.EditDistance(2))
                                     .PrefixLength(0)
                                     .Transpositions(true)
                                     .MaxExpansions(100)
                                     .Value(search_query)
                                )
                            ),

                            fs => fs
                            .Field(f => f.Description)
                            .RequireFieldMatch(true)
                            .Type(HighlighterType.Plain)
                            .PreTags("<strong class='blue'>")
                            .PostTags("</strong>")
                            .ForceSource()
                            .Fragmenter(HighlighterFragmenter.Span)
                            .HighlightQuery(q => q
                                .Fuzzy(m =>
                                    m.Field(fi => fi.Description)
                                     .Fuzziness(Fuzziness.EditDistance(2))
                                     .PrefixLength(0)
                                     .Transpositions(true)
                                     .MaxExpansions(100)
                                     .Value(search_query)
                                )
                            ),

                            fs => fs
                            .Field(f => f.UserChannelName)
                            .Type(HighlighterType.Plain)
                            .PreTags("<strong class='blue'>")
                            .PostTags("</strong>")
                            .ForceSource()
                            .Fragmenter(HighlighterFragmenter.Span)
                            .HighlightQuery(q => q
                                .Fuzzy(m =>
                                    m.Field(fi => fi.UserChannelName)
                                     .Fuzziness(Fuzziness.EditDistance(2))
                                     .PrefixLength(0)
                                     .Transpositions(true)
                                     .MaxExpansions(100)
                                     .Value(search_query)
                                )
                            ),

                            fs => fs
                            .Field(f => f.Hashtags)
                            .Type(HighlighterType.Plain)
                            .PreTags("<strong class='blue'>")
                            .PostTags("</strong>")
                            .ForceSource()
                            .Fragmenter(HighlighterFragmenter.Span)
                            .HighlightQuery(q => q
                                .Fuzzy(m =>
                                    m.Field(fi => fi.Hashtags)
                                     .Fuzziness(Fuzziness.EditDistance(2))
                                     .PrefixLength(0)
                                     .Transpositions(true)
                                     .MaxExpansions(100)
                                     .Value(search_query)
                                )
                            )
                        )
                    )
                    .Sort(so => so
                        .Ascending(a => a.Name)
                    )
                );
            }
            #endregion

            #region usersSearch
            if (search_etity == null || search_etity.Equals("users"))
            {
                usersResponse = await client.SearchAsync<ApplicationUserModel>(s => s
                .Index("users")
                .AllTypes()
                .From(page)
                .Size(10)
                .Query(q =>
                    q.Fuzzy(f => f
                        .Field(fi => fi.Firstname)
                        .Fuzziness(Fuzziness.EditDistance(2))
                        .PrefixLength(0)
                        .Transpositions(true)
                        .MaxExpansions(100)
                        .Value(search_query)
                    )
                    ||
                    q.Fuzzy(f => f
                        .Field(fi => fi.Lastname)
                        .Fuzziness(Fuzziness.EditDistance(2))
                        .PrefixLength(0)
                        .Transpositions(true)
                        .MaxExpansions(100)
                        .Value(search_query)
                    )
                    ||
                    q.Fuzzy(f => f
                        .Field(fi => fi.ChannelName)
                        .Fuzziness(Fuzziness.EditDistance(2))
                        .PrefixLength(0)
                        .Transpositions(true)
                        .MaxExpansions(100)
                        .Value(search_query)
                    )
                    ||
                    q.Fuzzy(f => f
                        .Field(fi => fi.ChannelDescription)
                        .Fuzziness(Fuzziness.EditDistance(2))
                        .PrefixLength(0)
                        .Transpositions(true)
                        .MaxExpansions(100)
                        .Value(search_query)
                    )
                )
                .Highlight(h => h
                    .PreTags("<strong>")
                    .PostTags("</strong>")
                    .Encoder(HighlighterEncoder.Html)
                    .Fields(fs =>
                        fs.Field(f => f.Firstname)
                        .Type(HighlighterType.Plain)
                        .PreTags("<strong class='blue'>")
                        .PostTags("</strong>")
                        .ForceSource()
                        .Fragmenter(HighlighterFragmenter.Span)
                        .HighlightQuery(q => q
                            .Fuzzy(m =>
                                m.Field(fi => fi.Firstname)
                                 .Fuzziness(Fuzziness.EditDistance(2))
                                 .PrefixLength(0)
                                 .Transpositions(true)
                                 .MaxExpansions(100)
                                 .Value(search_query)
                            )
                        ),

                        fs => fs
                        .Field(f => f.Lastname)
                        .RequireFieldMatch(true)
                        .Type(HighlighterType.Plain)
                        .PreTags("<strong class='blue'>")
                        .PostTags("</strong>")
                        .ForceSource()
                        .Fragmenter(HighlighterFragmenter.Span)
                        .HighlightQuery(q => q
                            .Fuzzy(m =>
                                m.Field(fi => fi.Lastname)
                                 .Fuzziness(Fuzziness.EditDistance(2))
                                 .PrefixLength(0)
                                 .Transpositions(true)
                                 .MaxExpansions(100)
                                 .Value(search_query)
                            )
                        ),

                        fs => fs
                        .Field(f => f.ChannelName)
                        .Type(HighlighterType.Plain)
                        .PreTags("<strong class='blue'>")
                        .PostTags("</strong>")
                        .ForceSource()
                        .Fragmenter(HighlighterFragmenter.Span)
                        .HighlightQuery(q => q
                            .Fuzzy(m =>
                                m.Field(fi => fi.ChannelName)
                                 .Fuzziness(Fuzziness.EditDistance(2))
                                 .PrefixLength(0)
                                 .Transpositions(true)
                                 .MaxExpansions(100)
                                 .Value(search_query)
                            )
                        ),

                        fs => fs
                        .Field(f => f.ChannelDescription)
                        .Type(HighlighterType.Plain)
                        .PreTags("<strong class='blue'>")
                        .PostTags("</strong>")
                        .ForceSource()
                        .Fragmenter(HighlighterFragmenter.Span)
                        .HighlightQuery(q => q
                            .Fuzzy(m =>
                                m.Field(fi => fi.ChannelDescription)
                                 .Fuzziness(Fuzziness.EditDistance(2))
                                 .PrefixLength(0)
                                 .Transpositions(true)
                                 .MaxExpansions(100)
                                 .Value(search_query)
                            )
                        )
                    )
                )
                .Sort(so => so
                    .Ascending(a => a.ChannelName)
                )
                );
            }
            #endregion

            
            List <SearchVideosViewModel> videoList = new List<SearchVideosViewModel>();
            List<SearchUsersViewModel> userList = new List<SearchUsersViewModel>();
            double totalPagesVideos = 0;
            double totalPagesUsers = 0;

            if (videosResponse != null)
            {
                foreach (IHit<VideoModel> video in videosResponse.Hits)
                {
                    SearchVideosViewModel searchVideo = new SearchVideosViewModel(
                        video.Source.Id, video.Source.Name, video.Source.Description, video.Source.UserChannelName,
                        video.Source.UserId, video.Source.Hashtags);

                    foreach (HighlightHit highLight in video.Highlights.Values)
                    {

                        foreach (String hl in highLight.Highlights)
                        {
                            switch (highLight.Field)
                            {
                                case "name":
                                    searchVideo.Name = hl;
                                    break;
                                case "description":
                                    searchVideo.Description = hl;
                                    break;
                                case "userChannelName":
                                    searchVideo.UserChannelName = hl;
                                    break;
                                case "hashtags":
                                    searchVideo.Hashtags = hl;
                                    break;
                            }
                        }
                    }
                    videoList.Add(searchVideo);
                }
                totalPagesVideos = Math.Ceiling((double)videosResponse.Total / 10);
            }

            if(usersResponse != null)
            {
                foreach (IHit<ApplicationUserModel> user in usersResponse.Hits)
                {
                    SearchUsersViewModel searchUsers = new SearchUsersViewModel(
                        user.Source.Id, user.Source.Firstname, user.Source.Lastname, user.Source.ChannelName,
                        user.Source.ChannelDescription, user.Source.ProfileImage, user.Source.DateOfBirth);

                    foreach (HighlightHit highLight in user.Highlights.Values)
                    {

                        foreach (String hl in highLight.Highlights)
                        {
                            switch (highLight.Field)
                            {
                                case "firstname":
                                    searchUsers.Firstname = hl;
                                    break;
                                case "lastname":
                                    searchUsers.Lastname = hl;
                                    break;
                                case "channelName":
                                    searchUsers.ChannelName = hl;
                                    break;
                                case "channelDescription":
                                    searchUsers.ChannelDescription = hl;
                                    break;
                            }
                        }
                    }
                    userList.Add(searchUsers);
                }
                totalPagesUsers = Math.Ceiling((double)usersResponse.Total / 10);
            }

            String contentType = Request.ContentType;


            if (contentType == null || !contentType.Equals("application/json"))
                return View("Index", new SearchViewModel(videoList, userList, totalPagesVideos, totalPagesUsers, search_query));
            else
                return Json(new SearchViewModel(videoList, userList, totalPagesVideos, totalPagesUsers, search_query));
        }

        public async Task<IActionResult> GetVideosIndex()
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("videos");

            ElasticClient client = new ElasticClient(settings);

            ISearchResponse<VideoModel> searchResponse = await client.SearchAsync<VideoModel>(s => s
                 .AllTypes()
                 .From(0)
                 .Size(10)
                 .Query(q => q
                    .MatchAll()
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

        public async Task<IActionResult> IndexVideo(VideoModel video)
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("videos");

            ElasticLowLevelClient lowlevelClient = new ElasticLowLevelClient(settings);

            List<Object> listObj = new List<object>();

            Object indexObj = new { index = new { _index = "videos", _type = "videomodel", _id = video.Id } };
            Object videoObj = new
            {
                id = video.Id,
                name = video.Name,
                description = video.Description,
                userChannelName = video.UserChannelName,
                userId = video.UserId,
                hashtags = video.Hashtags,
                dateCreatedOn = video.DateCreatedOn.ToString("yyyy-MM-dd")
            };

            listObj.Add(indexObj);
            listObj.Add(videoObj);

            StringResponse indexResponseList = await lowlevelClient.BulkAsync<StringResponse>(PostData.MultiJson(listObj));
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
                video.HashtagRelationships = await _hashtagRelationshipManager.GetByVideoId(video.Id, true);
                string hashtagName = string.Join(", ", video.HashtagRelationships.Select(x => x.Hashtag).Select(x => x.Name));

                Object indexObj = new { index = new { _index = "videos", _type = "videomodel", _id = video.Id } };
                Object videoObj = new
                {
                    id = video.Id,
                    name = video.Name,
                    description = video.Description,
                    userChannelName = video.User.ChannelName,
                    userId = video.UserId,
                    hashtags = hashtagName,
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
                    firstname = user.Firstname,
                    lastname = user.Lastname,
                    channelName = user.ChannelName,
                    channelDescription = user.ChannelDescription,
                    profileImage = user.ProfileImage
                };

                usersObj.Add(indexObj);
                usersObj.Add(userObj);
            }

            StringResponse vidoesResponse = await lowlevelClient.BulkAsync<StringResponse>(PostData.MultiJson(videosObj));
            StringResponse usersResponse = await lowlevelClient.BulkAsync<StringResponse>(PostData.MultiJson(usersObj));
            string videosResponseStream = vidoesResponse.Body;
            string usersResponseStream = usersResponse.Body;

            return Json((videosResponseStream, usersResponseStream));
        }

        public async Task<IActionResult> CreateIndex()
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("videos");

            ElasticClient client = new ElasticClient(settings);

            ICreateIndexResponse videoIndex = await client.CreateIndexAsync("videos", c => c
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
                                      .Fielddata(true)
                                 ).Text(t => t
                                      .Name(n => n.UserChannelName)
                                      .Analyzer("standard_english")
                                 ).Text(t => t
                                      .Name(n => n.UserId)
                                      .Analyzer("standard_english")
                                 ).Text(t => t
                                    .Name(n => n.Hashtags)
                                    .Analyzer("standard_english")
                                 )
                            )
                      )
                 )
            );

            ICreateIndexResponse usersIndex = await client.CreateIndexAsync("users", c => c
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
                                      .Fielddata(true)
                                 ).Text(t => t
                                      .Name(n => n.ChannelDescription)
                                      .Analyzer("standard_english")
                                      .Fielddata(true)
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
    }
}