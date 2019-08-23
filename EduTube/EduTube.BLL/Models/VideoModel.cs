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
      public string FilePath { get; set; }
      public bool AllowComments { get; set; }
      public bool Blocked { get; set; }
      public bool Deleted { get; set; }
      public string IvniteCode { get; set; }
      public TimeSpan Duration { get; set; }
      public string Thumbnail { get; set; }
      public ApplicationUserModel User { get; set; }
      public string UserId { get; set; }
      public string UserChannelName { get; set; }
      public string Hashtags { get; set; }
      public VideoVisibilityModel VideoVisibility { get; set; }
      public virtual List<ReactionModel> Reactions { get; set; }
      public virtual List<HashtagRelationshipModel> HashtagRelationships { get; set; }
      public virtual List<CommentModel> Comments { get; set; }
      public virtual List<ViewModel> Views { get; set; }
   }
}
