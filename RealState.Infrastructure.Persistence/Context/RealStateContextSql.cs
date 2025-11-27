using Microsoft.EntityFrameworkCore;
using RealState.Core.Domain.Entities;
using System.Reflection;

namespace RealState.Infrastructure.Persistence.Context
{
    public class RealStateContextSql : DbContext
    {
        public RealStateContextSql(DbContextOptions<RealStateContextSql> op) : base(op) { }

        public DbSet<PropertyUnit> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }
        public DbSet<ImprovementType> ImprovementTypes { get; set; }
        public DbSet<PropertyOffer> PropertyOffers { get; set; }
        public DbSet<UserFavoritePropertyUnit> UserFavoriteProperties { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
