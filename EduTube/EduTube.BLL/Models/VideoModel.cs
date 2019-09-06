using EduTube.BLL.Enums;
using EduTube.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class VideoModel : Model
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public DateTime DateCreatedOn { get; set; }
      public string YoutubeUrl { get; set; }
      public string FileName { get; set; }
      public bool AllowComments { get; set; }
      public bool Blocked { get; set; }
      public bool Deleted { get; set; }
      public string InvitationCode { get; set; }
      public TimeSpan Duration { get; set; }
      public string Thumbnail { get; set; }
      public ApplicationUserModel User { get; set; }
      public string UserId { get; set; }
      public string UserChannelName { get; set; }
      public string Tags { get; set; }
      public VideoVisibilityModel VideoVisibility { get; set; }
      public List<ReactionModel> Reactions { get; set; }
      public List<TagRelationshipModel> TagRelationships { get; set; }
      public List<CommentModel> Comments { get; set; }
      public List<ViewModel> Views { get; set; }
   }
}
