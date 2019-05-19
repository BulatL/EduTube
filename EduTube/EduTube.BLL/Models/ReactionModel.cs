using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
    public class ReactionModel
    {
        public int Id { get; set; }
        public DateTime DateCreatedOn { get; set; }
        public int EmoticonId { get; set; }
        public EmoticonModel Emoticon { get; set; }
        public string UserId { get; set; }
        public ApplicationUserModel User { get; set; }
        public int? VideoId { get; set; }
        public VideoModel Video { get; set; }
        public int? CommentId { get; set; }
        public CommentModel Comment { get; set; }
        public bool Deleted { get; set; }
    }
}
