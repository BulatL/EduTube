using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class ReactionModel : Model
   {
      public DateTime DateCreatedOn { get; set; }
      public int EmojiId { get; set; }
      public EmojiModel Emoji { get; set; }
      public string UserId { get; set; }
      public ApplicationUserModel User { get; set; }
      public int? VideoId { get; set; }
      public int? CommentId { get; set; }
      public bool Deleted { get; set; }
   }
}
