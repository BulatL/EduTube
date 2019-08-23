using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class SearchVideosViewModel
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public string UserChannelName { get; set; }
      public string UserId { get; set; }
      public string Hashtags { get; set; }

      public SearchVideosViewModel()
      {
      }

      public SearchVideosViewModel(int id, string name, string description, string userChannelName,
          string userId, string hashtags)
      {
         Id = id;
         Name = name;
         Description = description;
         UserChannelName = userChannelName;
         UserId = userId;
         Hashtags = hashtags;
      }
   }
}
