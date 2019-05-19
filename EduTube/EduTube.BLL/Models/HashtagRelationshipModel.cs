using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
    public class HashtagRelationshipModel
    {
        public int Id { get; set; }
        public int? VideoId { get; set; }
        public VideoModel Video { get; set; }
        public int? ChatId { get; set; }
        public ChatModel Chat { get; set; }
        public int? HashTagId { get; set; }
        public HashtagModel Hashtag { get; set; }
    }
}
