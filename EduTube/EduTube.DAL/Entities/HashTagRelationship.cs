using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
    public class HashTagRelationship
    {
        public int Id { get; set; }
        public int? VideoId { get; set; }
        public Video Video { get; set; }
        public int? ChatId { get; set; }
        public Chat Chat { get; set; }
        public int? HashTagId { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}
