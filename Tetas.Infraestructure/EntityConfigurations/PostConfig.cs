namespace Tetas.Infraestructure.EntityConfigurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PostConfig:  IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Body).IsRequired().HasMaxLength(4000);
        }
    }

    //public class PostConfig
    //{
    //    public PostConfig(EntityTypeBuilder<Post> entityBuilder)
    //    {
    //        entityBuilder.HasKey(x => x.Id);
    //        entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    //        entityBuilder.Property(x => x.Body).IsRequired().HasMaxLength(4000);
                       
    //    }
    //}
}
