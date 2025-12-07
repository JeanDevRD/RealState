using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Settings;
using RealState.Infrastructure.Shared.Services;

namespace RealState.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedLayer(this IServiceCollection services, IConfiguration confi) 
        {
            #region Configurations
            services.Configure<MailSettings>(confi.GetSection("MailSettings"));
            
            #endregion

            #region Services 
            services.AddScoped<IEmailService, EmailService>();
            #endregion
        }
    }
}
