using EduTube.BLL.Enums;
using EduTube.BLL.Models;
using EduTube.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class VideoCreateViewModel
   {
      public int Id { get; set; }
      [Required]
      public string Name { get; set; }
      [Required]
      public string Description { get; set; }
      public string YoutubeId { get; set; }
      [Display(Name = "Youtube embaded url")]
      public string YoutubeUrl { get; set; }
      public IFormFile Video { get; set; }
      [Display(Name = "Custom thumbnail")]
      public IFormFile Thumbnail { get; set; }
      public bool AllowComments { get; set; }
      public string InvitationCode { get; set; }
      public int VideoVisibility { get; set; }
      public string VideoDuration { get; set; }
      public string Tags { get; set; }
      public string OldVideo { get; set; }
      public string OldThumbnail { get; set; }
      public string YoutubeEmbeded { get; set; }
      public string OldFileName { get; set; }
      public TimeSpan OldDuration { get; set; }

      public VideoCreateViewModel()
      {
      }

      public VideoCreateViewModel(VideoModel model)
      {
         Id = model.Id;
         Name = model.Name;
         Description = model.Description;
         YoutubeUrl = model.YoutubeUrl;
         AllowComments = model.AllowComments;
         InvitationCode = model.InvitationCode;
         VideoVisibility = (int) model.VideoVisibility;
         VideoDuration = model.Duration.ToString();
         Tags = string.Join(",", model.TagRelationships.Select(x => x.Tag).Select(x => x.Name));
         OldVideo = model.FileName;
         OldThumbnail = model.Thumbnail;
         YoutubeUrl = model.YoutubeUrl;
         OldFileName = model.FileName;
      }

      public static VideoModel ConvertToModel(VideoCreateViewModel viewModel)
      {
         VideoModel model = new VideoModel();
         model.Name = viewModel.Name;
         model.Description = viewModel.Description;
         model.YoutubeUrl = "https://www.youtube.com/embed/" + viewModel.YoutubeId;
         model.AllowComments = viewModel.AllowComments;
         model.InvitationCode = viewModel.InvitationCode;
         model.VideoVisibility = (VideoVisibilityModel)viewModel.VideoVisibility;
         model.Deleted = false;
         model.DateCreatedOn = DateTime.Now;
         model.Duration = viewModel.OldDuration;

         return model;
      }
   }
}
