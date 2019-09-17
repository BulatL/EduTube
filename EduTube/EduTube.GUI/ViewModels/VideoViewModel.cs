using EduTube.BLL.Enums;
using EduTube.BLL.Models;
using EduTube.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class VideoViewModel
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Tags { get; set; }
      public string OwnerProfileImage { get; set; }
      public string OwnerChannelName { get; set; }
      public DateTime DateCreatedOn { get; set; }
      public string Description { get; set; }
      public string YoutubeUrl { get; set; }
      public string FileName { get; set; }
      [Display(Name = "Allow Comments")]
      public bool AllowComments { get; set; }
      public string UserId { get; set; }
      public VideoVisibilityModel Visibility { get; set; }
      public int? UserReactionOnVideo { get; set; }
      public List<ReactionViewModel> UserReactionOnComments { get; set; }
      public List<ReactionViewModel> Reactions { get; set; }
      public List<CommentViewModel> Comments { get; set; }

      public VideoViewModel()
      {
      }

      public VideoViewModel(VideoModel model, int? userReactionOnVideo, List<ReactionModel> userReactionOnComments)
      {
         Id = model.Id;
         Name = model.Name;
         Tags = model.Tags;
         OwnerProfileImage = model.User?.ProfileImage;
         OwnerChannelName = model.UserChannelName;
         DateCreatedOn = model.DateCreatedOn;
         Description = model.Description;
         YoutubeUrl = model.YoutubeUrl;
         FileName = model.FileName;
         AllowComments = model.AllowComments;
         Visibility = model.VideoVisibility;
         UserReactionOnVideo = userReactionOnVideo;
         UserReactionOnComments = ReactionViewModel.CopyToViewModels(userReactionOnComments);
         Reactions = ReactionViewModel.CopyToViewModels(model.Reactions);
         Comments = CommentViewModel.CopyToViewModels(model.Comments);
         UserId = model.UserId;
      }

      public static VideoModel CopyToModel(VideoViewModel viewModel)
      {
         VideoModel video = new VideoModel()
         {
            Id = viewModel.Id,
            Name = viewModel.Name,
            Tags = viewModel.Tags,
            DateCreatedOn = viewModel.DateCreatedOn,
            Description = viewModel.Description,
            YoutubeUrl = viewModel.YoutubeUrl,
            FileName = viewModel.FileName,
            AllowComments = viewModel.AllowComments,
            VideoVisibility = viewModel.Visibility
         };
         return video;
      }
   }
}
