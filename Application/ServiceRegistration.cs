using Microsoft.Extensions.DependencyInjection;
using RealState.Core.Application.Services;
using System.Reflection;

namespace Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services) 
        {

            #region Configuration
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            services.AddMediatR(services => services.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            #endregion

            #region Services
            services.AddScoped<PropertyUnitService>();
            services.AddScoped<PropertyTypeService>();
            services.AddScoped<SaleTypeService>();
            services.AddScoped<ImprovementTypeService>();
            services.AddScoped<AgentService>();
            services.AddScoped<ClientService>();
            services.AddScoped<AdminService>();
            services.AddScoped<DeveloperService>();
            services.AddScoped<ChatService>();
            services.AddScoped<MessageService>();
            services.AddScoped<PropertyOfferService>();
            services.AddScoped<FavoritePropertyServices>();
            #endregion


        }

    }
}
