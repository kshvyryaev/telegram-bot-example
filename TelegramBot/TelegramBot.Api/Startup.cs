using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Api.Configuration;
using TelegramBot.Api.Helpers;
using TelegramBot.Domain.Configuration;

namespace TelegramBot.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.ConfigureFactories();
            services.ConfigureCommands();
            services.ConfigureStateComponents(Configuration);
            services.ConfigureDomain(Configuration);
            services.ConfigureBot(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "bot-route",
                    template: SecurePathManager.SecureRoute,
                    defaults: new { controller = "Bot", action = "Execute" });
            });
        }
    }
}
