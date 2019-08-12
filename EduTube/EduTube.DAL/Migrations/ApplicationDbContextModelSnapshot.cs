﻿// <auto-generated />
using System;
using EduTube.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EduTube.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EduTube.DAL.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Blocked");

                    b.Property<string>("ChannelDescription");

                    b.Property<string>("ChannelName");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ProfileImage");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Chats");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Deleted = false,
                            Name = "Chat1"
                        },
                        new
                        {
                            Id = 2,
                            Deleted = false,
                            Name = "Chat2"
                        });
                });

            modelBuilder.Entity("EduTube.DAL.Entities.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChatId");

                    b.Property<DateTime>("DateCreatedOn");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Message");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateCreatedOn");

                    b.Property<bool>("Deleted");

                    b.Property<string>("UserId");

                    b.Property<int>("VideoId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Emoticon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("ImagePath");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Emoticons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Deleted = false,
                            ImagePath = "likeEmoticon.png",
                            Name = "Like"
                        },
                        new
                        {
                            Id = 2,
                            Deleted = false,
                            ImagePath = "dislikeEmoticon.png",
                            Name = "Dislike"
                        },
                        new
                        {
                            Id = 3,
                            Deleted = false,
                            ImagePath = "loveEmoticon.png",
                            Name = "Love"
                        },
                        new
                        {
                            Id = 4,
                            Deleted = false,
                            ImagePath = "wowEmoticon.png",
                            Name = "WoW"
                        },
                        new
                        {
                            Id = 5,
                            Deleted = false,
                            ImagePath = "laughtEmoticon.png",
                            Name = "Hahaha"
                        },
                        new
                        {
                            Id = 6,
                            Deleted = false,
                            ImagePath = "sadEmoticon.png",
                            Name = "Sad"
                        },
                        new
                        {
                            Id = 7,
                            Deleted = false,
                            ImagePath = "angryEmoticon.png",
                            Name = "Angry"
                        });
                });

            modelBuilder.Entity("EduTube.DAL.Entities.HashTagRelationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ChatId");

                    b.Property<int?>("HashTagId");

                    b.Property<int?>("VideoId");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("HashTagId");

                    b.HasIndex("VideoId");

                    b.ToTable("HashTagRelationships");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Hashtag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Hashtags");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Programming"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Frontend"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Backend"
                        },
                        new
                        {
                            Id = 4,
                            Name = "C#"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Java"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Python"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Web development"
                        },
                        new
                        {
                            Id = 8,
                            Name = "HTML"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Code"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Android "
                        },
                        new
                        {
                            Id = 11,
                            Name = "World code"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Engineering "
                        },
                        new
                        {
                            Id = 13,
                            Name = "Artificial intelligence"
                        });
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<bool>("Deleted");

                    b.Property<string>("RedirectPath");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Reaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CommentId");

                    b.Property<DateTime>("DateCreatedOn");

                    b.Property<bool>("Deleted");

                    b.Property<int>("EmoticonId");

                    b.Property<string>("UserId");

                    b.Property<int?>("VideoId");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("EmoticonId");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("SubscribedOnId");

                    b.Property<string>("SubscriberId");

                    b.HasKey("Id");

                    b.HasIndex("SubscribedOnId");

                    b.HasIndex("SubscriberId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AllowComments");

                    b.Property<bool>("Blocked");

                    b.Property<DateTime>("DateCreatedOn");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<TimeSpan>("Duration");

                    b.Property<string>("FilePath");

                    b.Property<string>("IvniteCode");

                    b.Property<string>("Name");

                    b.Property<string>("Thumbnail");

                    b.Property<string>("UserId");

                    b.Property<int>("VideoVisibility");

                    b.Property<string>("YoutubeUrl");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.View", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IpAddress");

                    b.Property<string>("UserId");

                    b.Property<int>("VideoId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("Views");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EduTube.DAL.Entities.ChatMessage", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Comment", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.Video", "Video")
                        .WithMany("Comments")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EduTube.DAL.Entities.HashTagRelationship", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.Chat", "Chat")
                        .WithMany("Hashtags")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.Hashtag", "Hashtag")
                        .WithMany()
                        .HasForeignKey("HashTagId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.Video", "Video")
                        .WithMany("HashtagRelationships")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Notification", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.ApplicationUser", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Reaction", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.Comment", "Comment")
                        .WithMany("Reactions")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.Emoticon", "Emoticon")
                        .WithMany()
                        .HasForeignKey("EmoticonId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.ApplicationUser", "User")
                        .WithMany("Reactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.Video", "Video")
                        .WithMany("Reactions")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Subscription", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.ApplicationUser", "SubscribedOn")
                        .WithMany("Subscribers")
                        .HasForeignKey("SubscribedOnId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.ApplicationUser", "Subscriber")
                        .WithMany("SubscribedOn")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EduTube.DAL.Entities.Video", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.ApplicationUser", "User")
                        .WithMany("Videos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EduTube.DAL.Entities.View", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EduTube.DAL.Entities.Video", "Video")
                        .WithMany("Views")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EduTube.DAL.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EduTube.DAL.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
