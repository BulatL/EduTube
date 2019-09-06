using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Chat : Entity
   {
      public string Name { get; set; }
      public bool Deleted { get; set; }
      public List<ChatMessage> Messages { get; set; }
      public List<TagRelationship> TagRelationspis { get; set; }
   }
}
