using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class TagRelationshipModel : Model
   {
      public int? VideoId { get; set; }
      public VideoModel Video { get; set; }
      public int? ChatId { get; set; }
      public ChatModel Chat { get; set; }
      public int? TagId { get; set; }
      public TagModel Tag { get; set; }
   }
}
