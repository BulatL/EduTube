﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class NotificationModel
   {
      public int Id { get; set; }
      public string Content { get; set; }
      public DateTime DateCreatedOn { get; set; }
      public string UserId { get; set; }
      public string UserProfileImage { get; set; }
      public bool Deleted { get; set; }
   }
}
