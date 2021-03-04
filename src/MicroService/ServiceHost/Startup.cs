using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Zaabee.Extensions.Configuration.Consul;
using Zaaby.DDD;

namespace ServiceHost
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "MicroService", Version = "v1"});
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

            //注册DDD各层
            services.AddDDD()
                //注册DbContext
                .AddDbContext(config)
                //注册Redis
                .AddRedis(config)
                //注册RabbitMQ
                .AddRabbitMq(config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroService v1"));
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<UowMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}