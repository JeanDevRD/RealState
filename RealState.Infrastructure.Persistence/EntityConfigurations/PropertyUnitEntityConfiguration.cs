using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class PropertyUnitEntityConfiguration : IEntityTypeConfiguration<PropertyUnit>
    {
        public void Configure(EntityTypeBuilder<PropertyUnit> builder)
        {
            #region Basic Configuration

            builder.HasKey(p => p.Id);
            builder.ToTable("PropertyUnits");

            #endregion

            #region Properties

            builder.Property(p => p.IdAgent).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PropertyTypeId).IsRequired();
            builder.Property(p => p.SaleTypeId).IsRequired();
            builder.Property(p => p.CodeProperty).IsRequired().HasMaxLength(10);
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Description).IsRequired().HasMaxLength(800);
            builder.Property(p => p.SizeM).IsRequired();
            builder.Property(p => p.Bedrooms).IsRequired();
            builder.Property(p => p.Bathrooms).IsRequired();
            builder.Property(p => p.StateProperty).IsRequired().HasDefaultValue((int)StateProperty.Available);

            builder.Property(p => p.Images)
                .HasConversion
                (
                    v => string.Join(",", v),
                    v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()
                ).HasMaxLength(1000).IsRequired();

            #endregion

            #region Relationships

            builder.HasMany(p => p.ImprovementTypes)
                .WithMany(i => i.PropertyUnits)
                .UsingEntity(j => j.ToTable("PropertyUnitImprovementTypes"));

            #endregion


        }
    }
}
