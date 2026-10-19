using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class PlayerShotConfiguration : IEntityTypeConfiguration<PlayerShot>
    {
        public void Configure(EntityTypeBuilder<PlayerShot> builder)
        {
            builder.ToTable("PlayerShots");
            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.Row).IsRequired();      // ✅
            builder.Property(ps => ps.Column).IsRequired();   // ✅
            builder.Property(ps => ps.PlayerId).IsRequired();

            builder.HasOne(ps => ps.Game)
                .WithMany(g => g.Shots)
                .HasForeignKey(ps => ps.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}