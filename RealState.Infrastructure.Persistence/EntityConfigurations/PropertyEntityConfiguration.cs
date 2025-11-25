using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class PropertyEntityConfiguration : IEntityTypeConfiguration<PropertyUnit>
    {
        public void Configure(EntityTypeBuilder<PropertyUnit> builder)
        {
            #region Basic Configuration

            builder.HasKey(p => p.Id);
            builder.ToTable("Properties");

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

            builder.OwnsMany(p => p.Images, img =>
            {
                img.ToTable("PropertyImages");
                img.Property<string>("Value").HasColumnName("ImageUrl").IsRequired();
                img.WithOwner().HasForeignKey("PropertyUnitId");
                img.HasKey("PropertyUnitId", "ImageUrl"); 
            });

            #endregion


        }
    }
}
