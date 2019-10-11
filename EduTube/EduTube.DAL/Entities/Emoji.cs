using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Emoji
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string ImagePath { get; set; }
      public bool Deleted { get; set; }
   }
}
