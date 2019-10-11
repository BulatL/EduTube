using EduTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class ChatMessageModel
   {
      public int Id { get; set; }
      public string Message { get; set; }
      public DateTime DateCreatedOn { get; set; }
      public ApplicationUserModel User { get; set; }
      public string UserId { get; set; }
      public int ChatId { get; set; }
      public bool Deleted { get; set; }
   }
}