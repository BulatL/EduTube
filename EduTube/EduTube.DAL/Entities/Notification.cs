using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string RedirectPath { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool Deleted { get; set; }
    }
}
