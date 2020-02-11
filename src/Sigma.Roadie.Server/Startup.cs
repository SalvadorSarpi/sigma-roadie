using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sigma.Roadie.Domain.DataModels;
using Sigma.Roadie.Server.Orquestrator;
using Sigma.Roadie.Services;

namespace Sigma.Roadie.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyOrigin();
            }));
            services.AddSignalR();

            /*
            var dbBuilder = new DbContextOptionsBuilder<RoadieEntities>().UseSqlServer(Configuration.GetConnectionString("RoadieEntities"));
            services.AddSingleton<DbContextOptions<RoadieEntities>>(dbBuilder.Options);*/
            // context database
            services.AddDbContext<RoadieEntities>(options => options.UseSqlServer(Configuration["ConnectionStrings:RoadieEntities"]));

            services.AddSingleton<OrchestratorClient>();

            // custom services
            services.AddTransient<SetlistService>();
            services.AddTransient<SceneService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHub<OrchestratorHub>("/orchestratorhub");
            });
        }
    }
}
