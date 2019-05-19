using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
    public class View
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public int? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int VideoId { get; set; }
        public Video Video { get; set; }
    }
}
