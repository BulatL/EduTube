using EduTube.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduTube.DAL.Data
{
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>//DbContext
   {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
      {
      }

      public DbSet<Chat> Chats { get; set; }
      public DbSet<ChatMessage> ChatMessages { get; set; }
      public DbSet<Comment> Comments { get; set; }
      public DbSet<Emoji> Emojis { get; set; }
      public DbSet<Tag> Tags { get; set; }
      public DbSet<Notification> Notifications { get; set; }
      public DbSet<Reaction> Reactions { get; set; }
      public DbSet<Video> Videos { get; set; }
      public DbSet<View> Views { get; set; }
      public DbSet<TagRelationship> TagRelationships { get; set; }
      public DbSet<Subscription> Subscriptions { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Chat>(ConfigureChat);
         modelBuilder.Entity<Notification>(ConfigureNotification);
         modelBuilder.Entity<Video>(ConfigureVideo);
         modelBuilder.Entity<TagRelationship>(ConfigureTagRelationship);
         modelBuilder.Entity<Subscription>(ConfigureSubscription);

         foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
         {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
         }
         base.OnModelCreating(modelBuilder);
         ApplicationDbContextExtensions.SeedData(modelBuilder);
      }

      private void ConfigureChat(EntityTypeBuilder<Chat> builder)
      {
         builder.HasMany(x => x.TagRelationships).WithOne(x => x.Chat);
      }
      private void ConfigureNotification(EntityTypeBuilder<Notification> builder)
      {
         builder.HasOne<ApplicationUser>().WithMany(x => x.Notifications).HasForeignKey(x => x.UserId);
      }
      private void ConfigureVideo(EntityTypeBuilder<Video> builder)
      {
         builder.HasMany(x => x.TagRelationships);
      }
      private void ConfigureTagRelationship(EntityTypeBuilder<TagRelationship> builder)
      {
         builder.HasOne(x => x.Chat).WithMany(x => x.TagRelationships).HasForeignKey(x => x.ChatId);
         builder.HasOne(x => x.Video).WithMany(x => x.TagRelationships).HasForeignKey(x => x.VideoId);
      }
      private void ConfigureSubscription(EntityTypeBuilder<Subscription> builder)
      {
         builder.HasOne(x => x.Subscriber).
             WithMany(x => x.SubscribedOn).
             HasForeignKey(x => x.SubscriberId);
         builder.HasOne(x => x.SubscribedOn).
             WithMany(x => x.Subscribers).
             HasForeignKey(x => x.SubscribedOnId);
      }
   }
}
