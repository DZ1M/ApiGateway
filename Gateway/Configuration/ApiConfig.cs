using Microsoft.AspNetCore.ResponseCompression;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IO.Compression;

namespace Gateway.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddOptions();

            services.AddOcelot(configuration);

            return services;
        }

        public static WebApplication UseApiConfiguration(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseResponseCompression();

            app.MapControllers();

            app.UseOcelot().Wait();

            return app;
        }
    }
}
