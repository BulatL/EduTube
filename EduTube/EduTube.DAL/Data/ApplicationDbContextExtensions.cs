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

      private static Emoji emoji1 = new Emoji { Id = 1, ImagePath = "likeEmoji.png", Name = "Like", Deleted = false };
      private static Emoji emoji2 = new Emoji { Id = 2, ImagePath = "dislikeEmoji.png", Name = "Dislike", Deleted = false };
      private static Emoji emoji3 = new Emoji { Id = 3, ImagePath = "loveEmoji.png", Name = "Love", Deleted = false };
      private static Emoji emoji4 = new Emoji { Id = 4, ImagePath = "wowEmoji.png", Name = "WoW", Deleted = false };
      private static Emoji emoji5 = new Emoji { Id = 5, ImagePath = "laughtEmoji.png", Name = "Hahaha", Deleted = false };
      private static Emoji emoji6 = new Emoji { Id = 6, ImagePath = "sadEmoji.png", Name = "Sad", Deleted = false };
      private static Emoji emoji7 = new Emoji { Id = 7, ImagePath = "angryEmoji.png", Name = "Angry", Deleted = false };

      private static Tag tag1 = new Tag { Id = 1, Name = "Programming" };
      private static Tag tag2 = new Tag { Id = 2, Name = "Frontend" };
      private static Tag tag3 = new Tag { Id = 3, Name = "Backend" };
      private static Tag tag4 = new Tag { Id = 4, Name = "C#" };
      private static Tag tag5 = new Tag { Id = 5, Name = "Java" };
      private static Tag tag6 = new Tag { Id = 6, Name = "Python" };
      private static Tag tag7 = new Tag { Id = 7, Name = "Web development" };
      private static Tag tag8 = new Tag { Id = 8, Name = "HTML" };
      private static Tag tag9 = new Tag { Id = 9, Name = "Code" };
      private static Tag tag10 = new Tag { Id = 10, Name = "Android " };
      private static Tag tag11 = new Tag { Id = 11, Name = "World code" };
      private static Tag tag12 = new Tag { Id = 12, Name = "Engineering " };
      private static Tag tag13 = new Tag { Id = 13, Name = "Artificial intelligence" };

      public static void SeedData(ModelBuilder builder)
      {

         builder.Entity<Chat>().HasData(
                chat1,
                chat2
            );

         builder.Entity<Emoji>().HasData(
             emoji1,
             emoji2,
             emoji3,
             emoji4,
             emoji5,
             emoji6,
             emoji7
             );

         builder.Entity<Tag>().HasData(
             tag1,
             tag2,
             tag3,
             tag4,
             tag5,
             tag6,
             tag7,
             tag8,
             tag9,
             tag10,
             tag11,
             tag12,
             tag13
             );

      }
   }
}
