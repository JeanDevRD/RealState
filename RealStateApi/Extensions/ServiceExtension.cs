using Asp.Versioning;
using Microsoft.OpenApi;

namespace RealStateApi.Extensions
{
    public static class ServiceExtension
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "RealState API",
                    Description = "An ASP.NET Core Web API for RealState Management System",
                    Contact = new OpenApiContact
                    {
                        Name = "Zorcis Calderon y Jean Carlos Mendoz",
                        Email = "zorciscalderon793@gmail.com",
                        Url = new Uri("https://www.itla.edu.do")
                    }

                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "RealState API",
                    Description = "An ASP.NET Core Web API for RealState Management System",
                    Contact = new OpenApiContact
                    {
                        Name = "Zorcis Calderon y Jean Carlos Mendoz",
                        Email = "zorciscalderon793@gmail.com",
                        Url = new Uri("https://www.itla.edu.do")
                    }

                });


                options.DescribeAllParametersInCamelCase();
            });
        }

        public static void AddApiVersioningConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine
                (
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            }).AddApiExplorer(opt => { 
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });        
        }
    }
}
