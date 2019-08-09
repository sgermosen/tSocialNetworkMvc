using Tetas.Infraestructure.Data;

namespace Tetas.Infraestructure
{
    using Domain.Entities;
    using EntityConfigurations;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ...

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);

            AddMyFilters(ref modelBuilder);

            // modelBuilder.ApplyAllConfigurations(); 
            //registering the configurations for all the classes

            modelBuilder.ApplyConfiguration(new ApplicationUserConfig());
            modelBuilder.ApplyConfiguration(new PostConfig());
            modelBuilder.ApplyConfiguration(new GroupConfig());
            modelBuilder.ApplyConfiguration(new GroupMemberConfig());
            modelBuilder.ApplyConfiguration(new GroupPostCommentConfig());
            modelBuilder.ApplyConfiguration(new GroupPostConfig());
            modelBuilder.ApplyConfiguration(new GroupTypeConfig());
            modelBuilder.ApplyConfiguration(new PostCommentConfig());
            modelBuilder.ApplyConfiguration(new PrivacyConfig());

            //  new ApplicationUserConfig(modelBuilder.Entity<ApplicationUser>());
            //new OwnerConfig(modelBuilder.Entity<Owner>());
            //new ShopConfig(modelBuilder.Entity<Shop>());

            ////if I want to remove the AspNet prefix from the identity tables
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var table = entityType.Relational().TableName;
                if (table.StartsWith("AspNet"))
                {
                    entityType.Relational().TableName = table.Substring(6);
                }
            };
          
        }

        private void AddMyFilters(ref ModelBuilder modelBuilder)
        {
             
            #region SoftDeleted
            modelBuilder.Entity<PostComment>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<Post>().HasQueryFilter(x => !x.Deleted);         
            modelBuilder.Entity<Group>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<GroupMember>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<GroupPost>().HasQueryFilter(x => !x.Deleted);           
            modelBuilder.Entity<GroupPostComment>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<GroupType>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<Privacy>().HasQueryFilter(x => !x.Deleted);
            #endregion
        }

        //public DbSet<Owner> Owners { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<GroupPost> GroupPosts { get; set; }

        public DbSet<GroupPostComment> GroupPostComments { get; set; }

        public DbSet<GroupType> GroupTypes { get; set; }

        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<Privacy> Privacies { get; set; }

    }
}
