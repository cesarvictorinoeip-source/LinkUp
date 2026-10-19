using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Games");
            builder.HasKey(g => g.Id);

            builder.HasOne(g => g.Player1).WithMany().HasForeignKey(g => g.Player1Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(g => g.Player2).WithMany().HasForeignKey(g => g.Player2Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}