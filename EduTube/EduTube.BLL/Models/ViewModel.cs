using System;
using System.Collections.Generic;
using System.Text;
using EduTube.DAL.Entities;

namespace EduTube.BLL.Models
{
   public class ViewModel : Model
   {
      public string IpAddress { get; set; }
      public string UserId { get; set; }
      public int VideoId { get; set; }
   }
}
