using Hangfire;
using HFWeb.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;
using System.Threading.Tasks;

using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors.Security;
using NSwag.AspNet.Owin;

[assembly: OwinStartup(typeof(HangfireSample.Startup))]

namespace HangfireSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["HangfireConnection"].ConnectionString;
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseSwaggerUi3(typeof(Startup).Assembly, settings =>
            {
                //針對RPC-Style WebAPI，指定路由包含Action名稱
                settings.GeneratorSettings.DefaultUrlTemplate =
                    "api/{controller}/{action}/{id?}";
                //可加入客製化調整邏輯
                settings.PostProcess = document =>
                {
                    document.Info.Title = "WebAPI 範例";
                };
            });
            // BackgroundJob.Schedule(() => new HangfireController().FunctionInsert(), TimeSpan.FromSeconds(10));

        }
    }
}