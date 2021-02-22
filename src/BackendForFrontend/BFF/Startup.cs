using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Npgsql;
using QueryService;

namespace BFF
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
            //批量注册QueryService
            services.Scan(scan => scan.FromAssemblyOf<UserQueryService>()
                .AddClasses(classes =>
                    classes.Where(@class => @class.Name.EndsWith("QueryService", StringComparison.OrdinalIgnoreCase)))
                .AsSelf().WithScopedLifetime());
            //由于Q端直接连数据库，以下示范不同数据库的注册
            services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(
                "Host=192.168.78.140;Username=postgres;Password=postgres;Database=postgres"));
            // services.AddScoped<IDbConnection>(_ => new SqlConnection(
            //     "server=192.168.78.140;database=TestDB;User=sa;password=123;Connect Timeout=30;Pooling=true;Min Pool Size=100;"));
            // services.AddScoped<IDbConnection>(_ => new MySqlConnection(
            //     "Database=TestDB;Data Source=192.168.78.140;User Id=root;Password=123;CharSet=utf8;port=3306"));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}