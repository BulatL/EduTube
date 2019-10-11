using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class TagRelationshipModel
   {
      public int Id { get; set; }
      public int? VideoId { get; set; }
      public VideoModel Video { get; set; }
      public int? ChatId { get; set; }
      public ChatModel Chat { get; set; }
      public int? TagId { get; set; }
      public TagModel Tag { get; set; }
   }
}
