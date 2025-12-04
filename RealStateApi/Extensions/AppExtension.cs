namespace RealStateApi.Extensions
{
    public static class AppExtension
    {
        public static void UseSwaggerConfiguration(this IApplicationBuilder app, IEndpointRouteBuilder routeBuilder)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt => { 
                var versionDescriptions = routeBuilder.DescribeApiVersions();

                if(versionDescriptions != null && versionDescriptions.Any())
                {
                    foreach (var apiVersion in versionDescriptions)
                    {
                        var url = $"/swagger/{apiVersion.GroupName}/swagger.json";
                        var name = $"RealState API - {apiVersion.GroupName.ToUpperInvariant()}";
                        opt.SwaggerEndpoint(url, name);
                    }
                }
            });
        }
         
    }
}
