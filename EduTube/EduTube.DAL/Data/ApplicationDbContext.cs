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
        public DbSet<Emoticon> Emoticons { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Notification> Notifications{ get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        //public DbSet<User> User { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<HashTagRelationship> HashTagRelationships { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(ConfigureChat);
            modelBuilder.Entity<ChatMessage>(ConfigureChatMessage);
            modelBuilder.Entity<Comment>(ConfigureComment);
            modelBuilder.Entity<Emoticon>(ConfigureEmoticon);
            modelBuilder.Entity<Hashtag>(ConfigureHashtag);
            modelBuilder.Entity<Notification>(ConfigureNotification);
            modelBuilder.Entity<Reaction>(ConfigureReaction);
            //modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<Video>(ConfigureVideo);
            modelBuilder.Entity<View>(ConfigureView);
            modelBuilder.Entity<HashTagRelationship>(ConfigureHashTagRelationship);
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
            builder.HasMany(x => x.Messages).WithOne(x => x.Chat);
            builder.HasMany(x => x.Hashtags).WithOne(x => x.Chat);

        }
        private void ConfigureChatMessage(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasOne(x => x.Chat).WithMany(x => x.Messages).HasForeignKey(x => x.ChatId);
        }
        private void ConfigureComment(EntityTypeBuilder<Comment> builder)
        {

        }
        private void ConfigureEmoticon(EntityTypeBuilder<Emoticon> builder)
        {

        }
        private void ConfigureHashtag(EntityTypeBuilder<Hashtag> builder)
        {
            //builder.HasMany(x => x.Videos);
            //builder.HasMany(x => x.Chats);
        }
        private void ConfigureNotification(EntityTypeBuilder<Notification> builder)
        {

        }
        private void ConfigureReaction(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.Reactions).HasForeignKey(x => x.UserId);
        }
        //private void ConfigureUser(EntityTypeBuilder<A> builder)
        //{
        //    //builder.HasMany(x => x.SubscribedOn);
        //}
        private void ConfigureVideo(EntityTypeBuilder<Video> builder)
        {
            builder.HasMany(x => x.HashtagRelationships);
            builder.HasMany(x => x.Views).WithOne(x => x.Video).HasForeignKey(x => x.VideoId);
        }
        private void ConfigureView(EntityTypeBuilder<View> builder)
        {

        }
        private void ConfigureHashTagRelationship(EntityTypeBuilder<HashTagRelationship> builder)
        {
            builder.HasOne(x => x.Chat).WithMany(x => x.Hashtags).HasForeignKey(x => x.ChatId);
            builder.HasOne(x => x.Video).WithMany(x => x.HashtagRelationships).HasForeignKey(x => x.VideoId);
        }
        private void ConfigureSubscription(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasOne(x => x.Subscriber).
                WithMany(x => x.Subscribers).
                HasForeignKey(x => x.SubscriberId);
            builder.HasOne(x => x.SubscribedOn).
                WithMany(x => x.SubscribedOn).
                HasForeignKey(x => x.SubscribedOnId);
            /*builder.HasKey(key => new {
                key.SubscriberId,
                key.SubscribedOnId});*/
        }
    }
}
