using EduTube.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class ReactionCreateViewModel
   {
      public int VideoId { get; set; }
      public int CommentId { get; set; }
      public int EmojiId { get; set; }

      public ReactionCreateViewModel()
      {
      }

      public static ReactionModel CopyToModel(ReactionCreateViewModel viewModel)
      {
         ReactionModel model = new ReactionModel();
         model.DateCreatedOn = DateTime.Now;
         model.EmojiId = viewModel.EmojiId;
         model.Deleted = false;
         if (viewModel.CommentId == 0)
            model.CommentId = null;
         else
            model.CommentId = viewModel.CommentId;
         if (viewModel.VideoId == 0)
            model.VideoId = null;
         else
            model.VideoId = viewModel.VideoId;

         return model;
      }
   }
}
