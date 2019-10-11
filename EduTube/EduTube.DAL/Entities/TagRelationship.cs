using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class TagRelationship
   {
      public int Id { get; set; }
      public int? VideoId { get; set; }
      public Video Video { get; set; }
      public int? ChatId { get; set; }
      public Chat Chat { get; set; }
      public int? TagId { get; set; }
      public Tag Tag { get; set; }
   }
}
