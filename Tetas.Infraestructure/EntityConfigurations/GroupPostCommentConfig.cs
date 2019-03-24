namespace Tetas.Infraestructure.EntityConfigurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GroupPostCommentConfig : IEntityTypeConfiguration<GroupPostComment>
    {
        public void Configure(EntityTypeBuilder<GroupPostComment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Body).IsRequired().HasMaxLength(4000);
        }
    }
}
