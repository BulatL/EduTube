using EduTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class CommentModel : Model
   {
      public string Content { get; set; }
      public DateTime DateCreatedOn { get; set; }
      public virtual ApplicationUserModel User { get; set; }
      public string UserId { get; set; }
      //public VideoModel Video { get; set; }
      public int VideoId { get; set; }
      public virtual List<ReactionModel> Reactions { get; set; }
      public bool Deleted { get; set; }
   }
}
