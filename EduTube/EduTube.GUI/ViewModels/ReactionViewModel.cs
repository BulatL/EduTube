using EduTube.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class ReactionViewModel
   {
      public int Id { get; set; }
      public int? VideoId { get; set; }
      public int? CommentId { get; set; }
      public string UserId { get; set; }
      public int EmoticonId { get; set; }

      public ReactionViewModel()
      {
      }

      public ReactionViewModel(ReactionModel model)
      {
         Id = model.Id;
         VideoId = model.VideoId;
         CommentId = model.CommentId;
         UserId = model.UserId;
         EmoticonId = model.EmoticonId;
      }

      public static List<ReactionViewModel> CopyToViewModels(List<ReactionModel> models)
      {
         if (models?.Any() == true)
            return models.Select(x => new ReactionViewModel(x)).ToList();
         else
            return new List<ReactionViewModel>();
      }
   }
}
