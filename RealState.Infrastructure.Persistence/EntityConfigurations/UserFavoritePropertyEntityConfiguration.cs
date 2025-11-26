using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Core.Domain.Entities;

namespace RealState.Infrastructure.Persistence.EntityConfiguration
{
    public class UserFavoritePropertyUnitEntityConfiguration : IEntityTypeConfiguration<UserFavoritePropertyUnit>
    {
        public void Configure(EntityTypeBuilder<UserFavoritePropertyUnit> builder)
        {
            #region Basic Configuration

            builder.HasKey(pt => pt.Id);
            builder.ToTable("UserFavoritePropertysUnit");

            #endregion

            #region Properties

            builder.Property(pt => pt.IdProperty).IsRequired();
            builder.Property(pt => pt.IdClient).IsRequired().HasMaxLength(100);

            #endregion

           

        }
    }
}
