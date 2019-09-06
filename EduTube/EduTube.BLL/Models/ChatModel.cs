using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class ChatModel : Model
   {
      public string Name { get; set; }
      public bool Deleted { get; set; }
      public List<ChatMessageModel> Messages { get; set; }
      public List<TagRelationshipModel> TagRelationspis { get; set; }
   }
}
