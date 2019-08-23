using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Comment : Entity
   {
      public string Content { get; set; }
      public DateTime DateCreatedOn { get; set; }
      [ForeignKey("UserId")]
      public ApplicationUser User { get; set; }
      public string UserId { get; set; }
     //[ForeignKey("VideoId")]
     // public Video Video { get; set; }
      public int VideoId { get; set; }
      public virtual List<Reaction> Reactions { get; set; }
      public bool Deleted { get; set; }
   }
}
