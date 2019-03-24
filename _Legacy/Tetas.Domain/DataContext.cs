namespace Tetas.Domain
{
    using DataEntities;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Status> Status { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Gender> Genders { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<UserPost> UserPosts { get; set; }

        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Privacy> Privacies { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<GroupType> GroupTypes { get; set; }

    }
}
