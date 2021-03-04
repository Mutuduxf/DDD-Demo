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
using Zaabee.Extensions.Configuration.Consul;
using Zaabee.StackExchangeRedis;
using Zaabee.StackExchangeRedis.Abstractions;
using Zaabee.StackExchangeRedis.MsgPack;

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "BackendForBrowser", Version = "v1"});
            });

            //通过Consul配置中心生成配置
            var configBuilder = new ConfigurationBuilder()
                .AddConsul(c =>
                {
                    c.Address = new Uri("http://192.168.78.140:8500");
                    c.Datacenter = "dc1";
                    c.WaitTime = TimeSpan.FromSeconds(30);
                });
            var config = configBuilder.Build();

            //批量注册QueryService
            services.Scan(scan => scan.FromAssemblyOf<UserQueryService>()
                .AddClasses(classes =>
                    classes.Where(@class => @class.Name.EndsWith("QueryService", StringComparison.OrdinalIgnoreCase)))
                .AsSelf().WithScopedLifetime());

            //Q端直连接从库
            services.AddScoped<IDbConnection>(
                _ => new NpgsqlConnection(config.GetSection("PgSqlStandby").Get<string>()));

            //Redis
            services.AddSingleton<IZaabeeRedisClient>(new ZaabeeRedisClient(
                config.GetSection("Redis").Get<string>(),
                TimeSpan.FromSeconds(30), new Serializer()));
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