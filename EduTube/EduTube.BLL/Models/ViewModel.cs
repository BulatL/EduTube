using System;
using System.Collections.Generic;
using System.Text;
using EduTube.DAL.Entities;

namespace EduTube.BLL.Models
{
    public class ViewModel
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public int? UserId { get; set; }
        public ApplicationUserModel User { get; set; }
        public int VideoId { get; set; }
        public VideoModel Video { get; set; }
    }
}
