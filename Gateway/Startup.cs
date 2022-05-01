using Gateway.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Gateway
{

    public class Startup : IStartup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddJsonFile($"configuration.{hostEnvironment.EnvironmentName}.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfiguration(Configuration);
        }
        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            app.UseApiConfiguration();

        }
        public void ConfigureLogging(ILoggingBuilder logging)
        {
            //logging.ClearProviders();
            //logging.AddConsole();
        }
    }

    public interface IStartup
    {
        IConfiguration Configuration { get; }
        void Configure(WebApplication app, IWebHostEnvironment environment);
        void ConfigureServices(IServiceCollection services);
        void ConfigureLogging(ILoggingBuilder logging);
    }

    public static class StartupExtensions
    {
        public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder WebAppBuilder) where TStartup : IStartup
        {
            var startup = Activator.CreateInstance(typeof(TStartup), WebAppBuilder.Configuration) as IStartup;

            if (startup == null) throw new ArgumentException("Class Startup.cs invalid!");

            startup.ConfigureServices(WebAppBuilder.Services);

            startup.ConfigureLogging(WebAppBuilder.Logging);

            var app = WebAppBuilder.Build();
            startup.Configure(app, app.Environment);
            app.Run();

            return WebAppBuilder;
        }
    }
}