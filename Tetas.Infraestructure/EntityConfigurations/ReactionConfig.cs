namespace Tetas.Infraestructure.EntityConfigurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ReactionConfig : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.PostId, x.OwnerId }).IsUnique();

            builder.HasOne(x => x.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(x => x.PostId);

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId);
        }
    }
}
