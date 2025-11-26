using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class PropertyTypeEntityConfiguration : IEntityTypeConfiguration<PropertyType>
    {
        public void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            #region Basic Configuration

            builder.HasKey(pt => pt.Id);
            builder.ToTable("PropertyTypes");

            #endregion

            #region Properties

            builder.Property(pt => pt.Name).IsRequired().HasMaxLength(60);
            builder.Property(pt => pt.Description).IsRequired().HasMaxLength(700);

            #endregion

            #region Relationships

            builder.HasMany(pt => pt.PropertyUnits)
                .WithOne(pu => pu.PropertyType)
                .HasForeignKey(pu => pu.PropertyTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

        }
    }
}
