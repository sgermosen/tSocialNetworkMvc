﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tetas.Infraestructure;

namespace Tetas.Infraestructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220403155311_TakingTheRequiredForTitle")]
    partial class TakingTheRequiredForTitle
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("Roles");
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

                    b.ToTable("RoleClaims");
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

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Bio");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Lastname")
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("NickName");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(200);

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

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("OwnerId");

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(500);

                    b.Property<long?>("PrivacyId");

                    b.Property<long?>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PrivacyId");

                    b.HasIndex("TypeId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.GroupMember", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ApplicationDate");

                    b.Property<bool>("Applied");

                    b.Property<bool>("Banned");

                    b.Property<bool>("Deleted");

                    b.Property<long?>("GroupId");

                    b.Property<int>("MemberType");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("State");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.GroupPost", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(4000);

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("Deleted");

                    b.Property<long?>("GroupId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("OwnerId");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("OwnerId");

                    b.ToTable("GroupPosts");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.GroupPostComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(4000);

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("OwnerId");

                    b.Property<long?>("PostId");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PostId");

                    b.ToTable("GroupPostComments");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.GroupType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("GroupTypes");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(4000);

                    b.Property<DateTime>("Date");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("OwnerId");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.PostComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(4000);

                    b.Property<DateTime>("Date");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("OwnerId");

                    b.Property<long?>("PostId");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PostId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.Privacy", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Privacies");
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
                    b.HasOne("Tetas.Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Tetas.Domain.Entities.ApplicationUser")
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

                    b.HasOne("Tetas.Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Tetas.Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tetas.Domain.Entities.Group", b =>
                {
                    b.HasOne("Tetas.Domain.Entities.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("Tetas.Domain.Entities.Privacy", "Privacy")
                        .WithMany()
                        .HasForeignKey("PrivacyId");

                    b.HasOne("Tetas.Domain.Entities.GroupType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.GroupMember", b =>
                {
                    b.HasOne("Tetas.Domain.Entities.Group", "Group")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupId");

                    b.HasOne("Tetas.Domain.Entities.ApplicationUser", "User")
                        .WithMany("GroupMembers")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.GroupPost", b =>
                {
                    b.HasOne("Tetas.Domain.Entities.Group", "Group")
                        .WithMany("GroupPosts")
                        .HasForeignKey("GroupId");

                    b.HasOne("Tetas.Domain.Entities.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.GroupPostComment", b =>
                {
                    b.HasOne("Tetas.Domain.Entities.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("Tetas.Domain.Entities.GroupPost", "Post")
                        .WithMany("GroupPostComments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.Post", b =>
                {
                    b.HasOne("Tetas.Domain.Entities.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Tetas.Domain.Entities.PostComment", b =>
                {
                    b.HasOne("Tetas.Domain.Entities.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("Tetas.Domain.Entities.Post", "Post")
                        .WithMany("PostComments")
                        .HasForeignKey("PostId");
                });
#pragma warning restore 612, 618
        }
    }
}
