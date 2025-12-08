using Microsoft.Extensions.DependencyInjection;
using RealState.Core.Application.Interfaces;
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
            services.AddScoped<IPropertyUnitService, PropertyUnitService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<ISaleTypeService, SaleTypeService>();
            services.AddScoped<IImprovementTypeService, ImprovementTypeService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IDeveloperService, DeveloperService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPropertyOfferService, PropertyOfferService>();
            services.AddScoped<IFavoritePropertyServices, FavoritePropertyServices>();
            #endregion


        }

    }
}
