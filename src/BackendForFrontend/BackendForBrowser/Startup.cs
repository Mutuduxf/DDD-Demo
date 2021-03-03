using System;
using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using QueryService;

namespace BackendForBrowser
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
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "BackendForBrowser", Version = "v1"}); });
            //批量注册QueryService
            services.Scan(scan => scan.FromAssemblyOf<UserQueryService>()
                .AddClasses(classes =>
                    classes.Where(@class => @class.Name.EndsWith("QueryService", StringComparison.OrdinalIgnoreCase)))
                .AsSelf().WithScopedLifetime());
            //Q端直连接从库，以下示范不同数据库的注册
            services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(
                "Host=192.168.78.142;Username=postgres;Password=postgres;Database=postgres"));
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendForBrowser v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}