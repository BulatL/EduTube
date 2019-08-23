using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Chat : Entity
   {
      public string Name { get; set; }
      public bool Deleted { get; set; }
      public virtual List<ChatMessage> Messages { get; set; }
      public virtual List<HashTagRelationship> Hashtags { get; set; }
   }
}
