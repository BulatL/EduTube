using EduTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
      public List<ReactionModel> Reactions { get; set; }
      //public List<ReactionModel> ValidReactions => Reactions.Where(x => !x.Deleted).ToList();
      public bool Deleted { get; set; }
   }
}
