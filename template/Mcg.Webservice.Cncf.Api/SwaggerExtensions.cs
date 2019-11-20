using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mcg.Webservice.Cncf.Api
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class SwaggerExtensions
    {
        private static readonly string title = typeof(SwaggerExtensions).Namespace;

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            return services.AddSwaggerGen((System.Action<SwaggerGenOptions>)(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = title, Version = "v1" });
            }));
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            return app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", title);
            });
        }
    }
}
