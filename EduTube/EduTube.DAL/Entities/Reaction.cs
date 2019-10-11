using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Reaction
   {
      public int Id { get; set; }
      public DateTime DateCreatedOn { get; set; }
      public int EmojiId { get; set; }
      public Emoji Emoji { get; set; }
      public string UserId { get; set; }
      public ApplicationUser User { get; set; }
      public int? VideoId { get; set; }
      public int? CommentId { get; set; }
      public bool Deleted { get; set; }
   }
}
