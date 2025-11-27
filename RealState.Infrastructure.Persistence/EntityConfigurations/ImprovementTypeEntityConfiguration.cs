using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class ImprovementTypeEntityConfiguration : IEntityTypeConfiguration<ImprovementType>
    {
        public void Configure(EntityTypeBuilder<ImprovementType> builder)
        {
            #region Basic Configuration

            builder.HasKey(pt => pt.Id);
            builder.ToTable("ImprovementTypes");

            #endregion

            #region Properties

            builder.Property(pt => pt.Name).IsRequired().HasMaxLength(60);
            builder.Property(pt => pt.Description).IsRequired().HasMaxLength(700);

            #endregion

            

        }
    }
}
