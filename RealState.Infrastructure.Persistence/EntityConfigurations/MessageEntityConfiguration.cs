using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            #region Basic Configuration

            builder.HasKey(m => m.Id);
            builder.ToTable("Messages");

            #endregion

            #region Properties

            builder.Property(m => m.IdChat).IsRequired();
            builder.Property(m => m.SenderId).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Content).IsRequired().HasMaxLength(1000);
            builder.Property(m => m.SentAt).IsRequired();

            #endregion

        }
    }
}
