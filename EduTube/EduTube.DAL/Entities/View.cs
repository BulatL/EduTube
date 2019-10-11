using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class View : Entity
   {
      public string IpAddress { get; set; }
      public string UserId { get; set; }
      public int VideoId { get; set; }
   }
}
