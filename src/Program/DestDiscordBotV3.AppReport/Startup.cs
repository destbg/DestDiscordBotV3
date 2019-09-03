using DestDiscordBotV3.AppReport.Hubs;
using DestDiscordBotV3.Data;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DestDiscordBotV3.AppReport
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var db = new MongoClient().GetDatabase("DestDiscordBotV3");
            services.AddSingleton(db);
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ReportHub>("/report");
            });

            if (!HybridSupport.IsElectronActive)
                return;

            Task.Run(async () =>
            {
                var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
                {
                    AutoHideMenuBar = true
                });
                browserWindow.SetTitle("Dest Discord Bot Reports");
            });
        }
    }
}
