using EduTube.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Video : Entity
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public DateTime DateCreatedOn { get; set; }
      //^(https|http):\/\/(?:www\.)?youtube.com\/embed\/[A-z0-9]+
      public string YoutubeUrl { get; set; }
      public string FileName { get; set; }
      public bool AllowComments { get; set; }
      public bool Blocked { get; set; }
      public bool Deleted { get; set; }
      public string InvitationCode { get; set; }
      public TimeSpan Duration { get; set; }
      public string Thumbnail { get; set; }
      public ApplicationUser User { get; set; }
      public string UserId { get; set; }
      public VideoVisibility VideoVisibility { get; set; }
      public List<Reaction> Reactions { get; set; }
      public List<TagRelationship> TagRelationships { get; set; }
      public List<Comment> Comments { get; set; }
      public List<View> Views { get; set; }
   }
}
