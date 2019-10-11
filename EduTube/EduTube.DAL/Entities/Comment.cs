﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Comment
   {
      public int Id { get; set; }
      public string Content { get; set; }
      public DateTime DateCreatedOn { get; set; }
      [ForeignKey("UserId")]
      public ApplicationUser User { get; set; }
      public string UserId { get; set; }
      public int VideoId { get; set; }
      public List<Reaction> Reactions { get; set; }
      public bool Deleted { get; set; }
   }
}
