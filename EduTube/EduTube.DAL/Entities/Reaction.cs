using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Reaction : Entity
   {
      public DateTime DateCreatedOn { get; set; }
      public int EmoticonId { get; set; }
      public Emoticon Emoticon { get; set; }
      public string UserId { get; set; }
      public ApplicationUser User { get; set; }
      public int? VideoId { get; set; }
      //public Video Video { get; set; }
      public int? CommentId { get; set; }
      //public Comment Comment { get; set; }
      public bool Deleted { get; set; }
   }
}
