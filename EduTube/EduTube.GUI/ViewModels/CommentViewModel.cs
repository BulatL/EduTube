using EduTube.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class CommentViewModel
   {
      public int Id { get; set; }
      public string OwnerProfileImage { get; set; }
      public string OwnerChannelName { get; set; }
      public DateTime DateCreatedOn { get; set; }
      public string Content { get; set; }
      public int VideoId { get; set; }
      public List<ReactionViewModel> Reactions { get; set; }

      public CommentViewModel()
      {
      }

      public CommentViewModel (CommentModel model)
      {
         Id = model.Id;
         OwnerProfileImage = model.User?.ProfileImage;
         OwnerChannelName = model.User?.ChannelName;
         DateCreatedOn = model.DateCreatedOn;
         Content = model.Content;
         VideoId = model.VideoId;
         Reactions = ReactionViewModel.CopyToViewModels(model.Reactions);
      }

      public static List<CommentViewModel> CopyToViewModels(List<CommentModel> models)
      {
         if (models?.Any() == true)
            return models.Select(x => new CommentViewModel(x)).ToList();
         else
            return new List<CommentViewModel>();
      }
   }
}
