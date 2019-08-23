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
      public string FirstHashtag { get; set; }
      public string SecondHashtag { get; set; }

      public HomeRecommendedVideos()
      {
      }

      public HomeRecommendedVideos(List<VideoModel> firstRecommended, List<VideoModel> secondRecommended,
          string firstHashtag, string secondHashtag)
      {
         FirstRecommended = firstRecommended;
         SecondRecommended = secondRecommended;
         FirstHashtag = firstHashtag;
         SecondHashtag = secondHashtag;
      }
   }
}
