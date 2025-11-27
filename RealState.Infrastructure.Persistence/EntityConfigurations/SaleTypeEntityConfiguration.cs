using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class SaleTypeEntityConfiguration : IEntityTypeConfiguration<SaleType>
    {
        public void Configure(EntityTypeBuilder<SaleType> builder)
        {
            #region Basic Configuration

            builder.HasKey(pt => pt.Id);
            builder.ToTable("SaleTypes");

            #endregion

            #region Properties

            builder.Property(pt => pt.Name).IsRequired().HasMaxLength(60);
            builder.Property(pt => pt.Description).IsRequired().HasMaxLength(700);

            #endregion

            #region Relationships

            builder.HasMany(pt => pt.PropertyUnits)
                .WithOne(pu => pu.SaleType)
                .HasForeignKey(pu => pu.SaleTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

        }
    }
}
