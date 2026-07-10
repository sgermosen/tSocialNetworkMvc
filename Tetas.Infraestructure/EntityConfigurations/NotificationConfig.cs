namespace Tetas.Infraestructure.EntityConfigurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ActorName).HasMaxLength(150);
            builder.Property(x => x.Message).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Url).HasMaxLength(300);
            builder.HasIndex(x => new { x.RecipientId, x.IsRead });

            builder.HasOne(x => x.Recipient)
                .WithMany()
                .HasForeignKey(x => x.RecipientId);
        }
    }
}
