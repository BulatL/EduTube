using EduTube.BLL.Enums;
using EduTube.BLL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class VideoEditViewModel
   {
      public int Id { get; set; }
      [Required]
      public string Name { get; set; }
      [Required]
      public string Description { get; set; }
      public string YoutubeUrl { get; set; }
      public bool AllowComments { get; set; }
      public string InvitationCode { get; set; }
      public int VideoVisibility { get; set; }
      public string Tags { get; set; }
      public string OldThumbnail { get; set; }
      public string OldFileName { get; set; }
      public TimeSpan OldDuration { get; set; }
      public IFormFile NewThumbnail { get; set; }
      public string UserId { get; set; }

      public VideoEditViewModel()
      {
      }

      public VideoEditViewModel(VideoModel model)
      {
         Id = model.Id;
         Name = model.Name;
         Description = model.Description;
         YoutubeUrl = model.YoutubeUrl;
         AllowComments = model.AllowComments;
         InvitationCode = model.InvitationCode;
         VideoVisibility = (int)model.VideoVisibility;
         Tags = string.Join(",", model.TagRelationships.Select(x => x.Tag).Select(x => x.Name));
         OldThumbnail = model.Thumbnail;
         YoutubeUrl = model.YoutubeUrl;
         OldFileName = model.FileName;
         UserId = model.UserId;
      }

      public static VideoModel ConvertToModel(VideoEditViewModel viewModel)
      {
         VideoModel model = new VideoModel();
         model.Id = viewModel.Id;
         model.Name = viewModel.Name;
         model.Description = viewModel.Description;
         model.YoutubeUrl = viewModel.YoutubeUrl;
         model.AllowComments = viewModel.AllowComments;
         model.InvitationCode = viewModel.InvitationCode;
         model.VideoVisibility = (VideoVisibilityModel)viewModel.VideoVisibility;
         model.Deleted = false;
         model.DateCreatedOn = DateTime.Now;
         model.Duration = viewModel.OldDuration;
         model.FileName = viewModel.OldFileName;
         model.Thumbnail = viewModel.OldThumbnail;
         model.UserId = viewModel.UserId;
         model.TagRelationships = new List<TagRelationshipModel>();

         return model;
      }
   }
}
