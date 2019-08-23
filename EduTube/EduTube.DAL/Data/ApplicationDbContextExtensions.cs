using EduTube.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduTube.DAL.Data
{
   public static class ApplicationDbContextExtensions
   {

      private static Chat chat1 = new Chat { Id = 1, Name = "Chat1", Deleted = false };
      private static Chat chat2 = new Chat { Id = 2, Name = "Chat2", Deleted = false };

      private static Emoticon emoticon1 = new Emoticon { Id = 1, ImagePath = "likeEmoticon.png", Name = "Like", Deleted = false };
      private static Emoticon emoticon2 = new Emoticon { Id = 2, ImagePath = "dislikeEmoticon.png", Name = "Dislike", Deleted = false };
      private static Emoticon emoticon3 = new Emoticon { Id = 3, ImagePath = "loveEmoticon.png", Name = "Love", Deleted = false };
      private static Emoticon emoticon4 = new Emoticon { Id = 4, ImagePath = "wowEmoticon.png", Name = "WoW", Deleted = false };
      private static Emoticon emoticon5 = new Emoticon { Id = 5, ImagePath = "laughtEmoticon.png", Name = "Hahaha", Deleted = false };
      private static Emoticon emoticon6 = new Emoticon { Id = 6, ImagePath = "sadEmoticon.png", Name = "Sad", Deleted = false };
      private static Emoticon emoticon7 = new Emoticon { Id = 7, ImagePath = "angryEmoticon.png", Name = "Angry", Deleted = false };

      private static Hashtag hashtag1 = new Hashtag { Id = 1, Name = "Programming" };
      private static Hashtag hashtag2 = new Hashtag { Id = 2, Name = "Frontend" };
      private static Hashtag hashtag3 = new Hashtag { Id = 3, Name = "Backend" };
      private static Hashtag hashtag4 = new Hashtag { Id = 4, Name = "C#" };
      private static Hashtag hashtag5 = new Hashtag { Id = 5, Name = "Java" };
      private static Hashtag hashtag6 = new Hashtag { Id = 6, Name = "Python" };
      private static Hashtag hashtag7 = new Hashtag { Id = 7, Name = "Web development" };
      private static Hashtag hashtag8 = new Hashtag { Id = 8, Name = "HTML" };
      private static Hashtag hashtag9 = new Hashtag { Id = 9, Name = "Code" };
      private static Hashtag hashtag10 = new Hashtag { Id = 10, Name = "Android " };
      private static Hashtag hashtag11 = new Hashtag { Id = 11, Name = "World code" };
      private static Hashtag hashtag12 = new Hashtag { Id = 12, Name = "Engineering " };
      private static Hashtag hashtag13 = new Hashtag { Id = 13, Name = "Artificial intelligence" };

      public static void SeedData(ModelBuilder builder)
      {

         builder.Entity<Chat>().HasData(
                chat1,
                chat2
            );

         builder.Entity<Emoticon>().HasData(
             emoticon1,
             emoticon2,
             emoticon3,
             emoticon4,
             emoticon5,
             emoticon6,
             emoticon7
             );

         builder.Entity<Hashtag>().HasData(
             hashtag1,
             hashtag2,
             hashtag3,
             hashtag4,
             hashtag5,
             hashtag6,
             hashtag7,
             hashtag8,
             hashtag9,
             hashtag10,
             hashtag11,
             hashtag12,
             hashtag13
             );

      }
   }
}
