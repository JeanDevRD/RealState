using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class ChatEntityConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            #region Basic Configuration

            builder.HasKey(c => c.Id);
            builder.ToTable("Chats");

            #endregion

            #region Properties

            builder.Property(c => c.IdAgent).IsRequired().HasMaxLength(100);
            builder.Property(c => c.IdClient).IsRequired().HasMaxLength(100);
            builder.Property(c => c.IdProperty).IsRequired();

            #endregion

            #region Relationships

            builder.HasOne(c => c.Property)
                .WithMany(p => p.Chats)
                .HasForeignKey(c => c.IdProperty)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(c => c.IdChat)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion


        }
    }
}
