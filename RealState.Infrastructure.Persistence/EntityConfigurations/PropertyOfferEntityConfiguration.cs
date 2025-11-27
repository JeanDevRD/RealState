using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class PropertyOfferEntityConfiguration : IEntityTypeConfiguration<PropertyOffer>
    {
        public void Configure(EntityTypeBuilder<PropertyOffer> builder)
        {
            #region Basic Configuration

            builder.HasKey(po => po.Id);
            builder.ToTable("PropertyOfferts");

            #endregion

            #region Properties

            builder.Property(po => po.IdClient).IsRequired().HasMaxLength(100);
            builder.Property(po => po.IdProperty).IsRequired();
            builder.Property(po => po.OfferDate).IsRequired();
            builder.Property(po => po.OfferAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(po => po.OfferStatus).IsRequired().HasDefaultValue((int)OfferStatus.Pending);

            #endregion

            #region Relationships

            builder.HasOne(po => po.Property)
                .WithMany(p => p.PropertyOffers)
                .HasForeignKey(po => po.IdProperty)
                .OnDelete(DeleteBehavior.Restrict); 

            #endregion

        }
    }
}
