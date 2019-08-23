using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class EmoticonModel : Model
   {
      public string Name { get; set; }
      public string ImagePath { get; set; }
      public bool Deleted { get; set; }
   }
}
