using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.GUI.ViewModels;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Authorization;
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
      private ITagRelationshipManager _tagRelationshipManager;

      public ElasticsearchController(IVideoManager videoManager, IApplicationUserManager applicationUserManager,
          ITagRelationshipManager tagRelationshipManager)
      {
         _videoManager = videoManager;
         _applicationUserManager = applicationUserManager;
         _tagRelationshipManager = tagRelationshipManager;
      }

      [Route("Search")]
      public async Task<IActionResult> Search(string search_query, string search_etity, int? page)
      {
         ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"));
         ElasticClient client = new ElasticClient(settings);

         if (page != null)
            page = (page - 1) * 10;
         else
            page = 0;
         ISearchResponse<VideoModel> videosResponse = null;
         ISearchResponse<ApplicationUserModel> usersResponse = null;
         //split search query by white space. So you can search all words in search query
         List<String> search_querys = search_query.Split(null).ToList();

         search_query = search_query.Replace("-", " ");

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
                    q.MatchPhrasePrefix(m => m
                        .Field(fi => fi.Name)
                        .Query(search_query)
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
                    q.MatchPhrasePrefix(m => m
                        .Field(fi => fi.Description)
                        .Query(search_query)
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
                    q.MatchPhrasePrefix(m => m
                        .Field(fi => fi.UserChannelName)
                        .Query(search_query)
                    )
                    ||
                    q.Fuzzy(f => f
                        .Field(fi => fi.Tags)
                        .Fuzziness(Fuzziness.EditDistance(2))
                        .PrefixLength(0)
                        .Transpositions(true)
                        .MaxExpansions(100)
                        .Value(search_query)
                    )
                    ||
                    q.MatchPhrasePrefix(m => m
                        .Field(fi => fi.Tags)
                        .Query(search_query)
                    )
                )
                /* .Highlight(h => h
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
                         .Field(f => f.Tags)
                         .Type(HighlighterType.Plain)
                         .PreTags("<strong class='blue'>")
                         .PostTags("</strong>")
                         .ForceSource()
                         .Fragmenter(HighlighterFragmenter.Span)
                         .HighlightQuery(q => q
                             .Fuzzy(m =>
                                 m.Field(fi => fi.Tags)
                                  .Fuzziness(Fuzziness.EditDistance(2))
                                  .PrefixLength(0)
                                  .Transpositions(true)
                                  .MaxExpansions(100)
                                  .Value(search_query)
                             )
                         )
                     )
                 )*/
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
                q.MatchPhrasePrefix(m => m
                    .Field(fi => fi.Firstname)
                    .Query(search_query)
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
                q.MatchPhrasePrefix(m => m
                    .Field(fi => fi.Lastname)
                    .Query(search_query)
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
                q.MatchPhrasePrefix(m => m
                    .Field(fi => fi.ChannelName)
                    .Query(search_query)
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
                ||
                q.MatchPhrasePrefix(m => m
                    .Field(fi => fi.ChannelDescription)
                    .Query(search_query)
                )
            )
            /*.Highlight(h => h
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
            )*/
            .Sort(so => so
                .Ascending(a => a.ChannelName)
            )
            );
         }
         #endregion

         List<SearchVideosViewModel> videoList = new List<SearchVideosViewModel>();
         List<SearchUsersViewModel> userList = new List<SearchUsersViewModel>();
         double totalPagesVideos = 0;
         double totalPagesUsers = 0;

         if (videosResponse != null)
         {
            foreach (IHit<VideoModel> video in videosResponse.Hits)
            {
               SearchVideosViewModel searchVideo = new SearchVideosViewModel(
                   video.Source.Id, video.Source.Name, video.Source.Description, video.Source.UserChannelName,
                   video.Source.UserId, video.Source.Tags, video.Source.Thumbnail, video.Source.DateCreatedOn);

               /*foreach (HighlightHit highLight in video.Highlights.Values)
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
                           searchVideo.Tags = hl;
                           break;
                     }
                  }
               }*/
               videoList.Add(searchVideo);
            }
            totalPagesVideos = Math.Ceiling((double)videosResponse.Total / 10);
         }

         if (usersResponse != null)
         {
            foreach (IHit<ApplicationUserModel> user in usersResponse.Hits)
            {
               SearchUsersViewModel searchUsers = new SearchUsersViewModel(
                   user.Source.Id, user.Source.Firstname, user.Source.Lastname, user.Source.ChannelName,
                   user.Source.ChannelDescription, user.Source.ProfileImage, user.Source.DateOfBirth);

               /*foreach (HighlightHit highLight in user.Highlights.Values)
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
               }*/
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

      [Authorize]
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
            thumbnail = video.Thumbnail,
            name = video.Name,
            description = video.Description,
            userChannelName = video.UserChannelName,
            userId = video.UserId,
            tags = video.Tags,
            dateCreatedOn = video.DateCreatedOn.ToString("yyyy-MM-dd")
         };

         listObj.Add(indexObj);
         listObj.Add(videoObj);

         StringResponse indexResponseList = await lowlevelClient.BulkAsync<StringResponse>(PostData.MultiJson(listObj));
         string responseStream = indexResponseList.Body;

         return Json(responseStream);
      }
      [Authorize]

      public async Task<IActionResult> IndexUser(ApplicationUserModel user)
      {
         ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
         .DefaultIndex("videos");

         ElasticLowLevelClient lowlevelClient = new ElasticLowLevelClient(settings);

         List<Object> listObj = new List<object>();

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

         listObj.Add(indexObj);
         listObj.Add(userObj);

         StringResponse indexResponseList = await lowlevelClient.BulkAsync<StringResponse>(PostData.MultiJson(listObj));
         string responseStream = indexResponseList.Body;

         return Json(responseStream);
      }
      [Authorize]

      public async Task<IActionResult> UpdateVideo(VideoModel video)
      {
         var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
         .DefaultIndex("videos");

         var client = new ElasticClient(settings);

         dynamic updateFields = new ExpandoObject();
         updateFields.id = video.Id;
         updateFields.thumbnail = video.Thumbnail;
         updateFields.name = video.Name;
         updateFields.description = video.Description;
         updateFields.userChannelName = video.UserChannelName;
         updateFields.userId = video.UserId;
         updateFields.tags = video.Tags;
         updateFields.dateCreatedOn = video.DateCreatedOn;


         var response = await client.UpdateAsync<VideoModel, dynamic>(new DocumentPath<VideoModel>(video.Id), u => u
         .Index("videos").Doc(updateFields));

         return Json(response);
      }
      [Authorize]

      public async Task<IActionResult> UpdateUser(ApplicationUserModel user)
      {
         var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
         .DefaultIndex("users");

         var client = new ElasticClient(settings);

         dynamic updateFields = new ExpandoObject();
         updateFields.id = user.Id;
         updateFields.dateOfBirth = user.DateOfBirth;
         updateFields.firstname = user.Firstname;
         updateFields.lastname = user.Lastname;
         updateFields.channelName = user.ChannelName;
         updateFields.channelDescription = user.ChannelDescription;
         updateFields.profileImage = user.ProfileImage;


         var response = await client.UpdateAsync<ApplicationUserModel, dynamic>(new DocumentPath<ApplicationUserModel>(user.Id), u => u
         .Index("users").Doc(updateFields));

         return Json(response);
      }
      [Authorize]

      [HttpDelete]
      public JsonResult DeleteDocument(string id, string index, string type)
      {
         var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
         .DefaultIndex(index);

         var client = new ElasticClient(settings);
         var response = client.Delete<VideoModel>(id, d => d
                                             .Index(index)
                                             .Type(type)
                                          );

         return Json(response);
      }

      [Authorize(Roles = "Admin")]

      public async Task<IActionResult> IndexFromDb()
      {

         ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200"))
         .DefaultIndex("videos");

         ElasticLowLevelClient lowlevelClient = new ElasticLowLevelClient(settings);
         ElasticClient client = new ElasticClient(settings);

         List<VideoModel> videos = await _videoManager.GetAll();
         List<ApplicationUserModel> users = await _applicationUserManager.GetAll();


         List<Object> videosObj = new List<object>();
         List<Object> usersObj = new List<object>();

         foreach (VideoModel video in videos)
         {
            IExistsResponse exist = client.DocumentExists(new DocumentExistsRequest("videos", "videomodel", video.Id));

            if (!exist.Exists)
            {
               video.User = await _applicationUserManager.GetById(video.UserId, false);
               video.TagRelationships = await _tagRelationshipManager.GetByVideoId(video.Id, true);
               string hashtagName = string.Join(", ", video.TagRelationships.Select(x => x.Tag).Select(x => x.Name));

               Object indexObj = new { index = new { _index = "videos", _type = "videomodel", _id = video.Id } };
               Object videoObj = new
               {
                  id = video.Id,
                  thumbnail = video.Thumbnail,
                  name = video.Name,
                  description = video.Description,
                  userChannelName = video.User.ChannelName,
                  userId = video.UserId,
                  tags = hashtagName,
                  dateCreatedOn = video.DateCreatedOn.ToString("yyyy-MM-dd")
               };

               videosObj.Add(indexObj);
               videosObj.Add(videoObj);
            }
         }

         foreach (ApplicationUserModel user in users)
         {
            IExistsResponse exist = client.DocumentExists(new DocumentExistsRequest("users", "applicationusermodel", user.Id.ToString()));

            if (!exist.Exists)
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
         }
         string videosResponseStream = "";
         string usersResponseStream = "";

         if (videosObj.Count() > 0)
         {
            StringResponse vidoesResponse = await lowlevelClient.BulkAsync<StringResponse>(PostData.MultiJson(videosObj));
            videosResponseStream = vidoesResponse.Body;
         }
         if (usersObj.Count() > 0)
         {
            StringResponse usersResponse = await lowlevelClient.BulkAsync<StringResponse>(PostData.MultiJson(usersObj));
            usersResponseStream = usersResponse.Body;
         }

         return Json((videosResponseStream, usersResponseStream));
      }

      [Authorize(Roles = "Admin")]

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
                                   .Name(n => n.Thumbnail)
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
                                 .Name(n => n.Tags)
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