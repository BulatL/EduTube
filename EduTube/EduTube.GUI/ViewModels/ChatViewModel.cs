using EduTube.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class ChatViewModel
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public bool Delete { get; set; }
      public string Tags { get; set; }

      public ChatViewModel()
      {
      }

      public ChatViewModel(ChatModel chat)
      {
         Id = chat.Id;
         Name = chat.Name;
         Delete = chat.Deleted;
         Tags = string.Join(",", chat.TagRelationships.Select(x => x.Tag).Select(x => x.Name));
      }

      public static ChatModel CopyToModel(ChatViewModel viewModel)
      {
         ChatModel chat = new ChatModel()
         {
            Id = viewModel.Id,
            Name = viewModel.Name,
            Deleted = false,
            TagRelationships = new List<TagRelationshipModel>()
         };
         return chat;
      }
   }
}
