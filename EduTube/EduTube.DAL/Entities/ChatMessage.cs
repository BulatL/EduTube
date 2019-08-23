using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class ChatMessage : Entity
   {
      public string Message { get; set; }
      public DateTime DateCreatedOn { get; set; }
      [ForeignKey("UserId")]
      public ApplicationUser User { get; set; }
      public string UserId { get; set; }
      //[ForeignKey("ChatId")]
      //public Chat Chat { get; set; }
      public int ChatId { get; set; }
      public bool Deleted { get; set; }
   }
}
