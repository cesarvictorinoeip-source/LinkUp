using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class ShipConfiguration : IEntityTypeConfiguration<Ship>
    {
        public void Configure(EntityTypeBuilder<Ship> builder)
        {
            builder.ToTable("Ships");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Type).IsRequired().HasMaxLength(50);
            builder.Property(s => s.PlayerId).IsRequired();
            builder.Property(s => s.StartX).IsRequired();   // ✅
            builder.Property(s => s.StartY).IsRequired();   // ✅
            builder.Property(s => s.IsVertical).IsRequired(); // ✅

            builder.HasOne(s => s.Game)
                .WithMany(g => g.Ships)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}