using EduTube.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class HomeRecommendedVideos
   {
      public List<VideoModel> FirstRecommended { get; set; }
      public List<VideoModel> SecondRecommended { get; set; }
      public string FirstTag { get; set; }
      public string SecondTag { get; set; }

      public HomeRecommendedVideos()
      {
      }

      public HomeRecommendedVideos(List<VideoModel> firstRecommended, List<VideoModel> secondRecommended,
          string firstTag, string secondTag)
      {
         FirstRecommended = firstRecommended;
         SecondRecommended = secondRecommended;
         FirstTag = firstTag;
         SecondTag = secondTag;
      }
   }
}
