using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendships");
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.Sender)
                .WithMany()
                .HasForeignKey(f => f.IdUserOne) // ✅
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Receiver)
                .WithMany()
                .HasForeignKey(f => f.IdUserTwo) // ✅
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}