﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Notification : Entity
   {
      public string Content { get; set; }
      public DateTime DateCreatedOn { get; set; }
      public string UserId { get; set; }
      public string UserProfileImage { get; set; }
      public bool Deleted { get; set; }
   }
}
