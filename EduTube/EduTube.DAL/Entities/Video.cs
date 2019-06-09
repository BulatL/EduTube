using EduTube.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EduTube.DAL.Entities
{
    public class Video
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreatedOn { get; set; }
        //^(https|http):\/\/(?:www\.)?youtube.com\/embed\/[A-z0-9]+
        public string YoutubeUrl { get; set; }
        public string FilePath { get; set; }
        public bool AllowComments { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }
        public string IvniteCode { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public VideoVisibility VideoVisibility { get; set; }
        public virtual List<Reaction> Reactions { get; set; }
        public virtual List<HashTagRelationship> HashtagRelationships { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public List<View> Views { get; set; }
    }
}
