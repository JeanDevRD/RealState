using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;
using RealState.Infrastructure.Persistence.Repositories;

namespace RealState.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection Services, IConfiguration confi) 
        {
            #region Context
            var connectionString = confi.GetConnectionString("DefaultConnection");

            Services.AddDbContext<RealStateContextSql>
            (
                (ServiceProvider, Opt) =>
                {

                    Opt.EnableSensitiveDataLogging();
                    Opt.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly(typeof(RealStateContextSql).Assembly.FullName));
                },
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Scoped

            );
            #endregion

            #region Services
            Services.AddScoped<IPropertyUnitRepository, PropertyUnitRepository>();
            Services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            Services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
            Services.AddScoped<IImprovementTypeRepository, ImprovementTypeRepository>();
            Services.AddScoped<IChatRepository, ChatRepository>();
            Services.AddScoped<IMessageRepository, MessageRepository>();
            Services.AddScoped<IPropertyOfferRepository, PropertyOfferRepository>();
            Services.AddScoped<IUserFavoritePropertyUnitRepository, UserFavoritePropertyUnitRepository>();
            #endregion
        }
    }

}
