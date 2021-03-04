using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Repository;
using Zaabee.Extensions.Configuration.Consul;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.StackExchangeRedis;
using Zaabee.StackExchangeRedis.Abstractions;
using Zaabee.StackExchangeRedis.MsgPack;
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

            //通过Consul配置中心生成配置
            var configBuilder = new ConfigurationBuilder()
                .AddConsul(c =>
                {
                    c.Address = new Uri("http://192.168.78.140:8500");
                    c.Datacenter = "dc1";
                    c.WaitTime = TimeSpan.FromSeconds(30);
                });
            var config = configBuilder.Build();

            //自动注册DDD各层
            services.AddDDD();

            //注册EF用于C端仓储层
            services.AddDbContext<CustomDbContext>(options =>
                options.UseNpgsql(config.GetSection("PgSqlPrimary").Get<string>()));
            //使用上面已注册的pgsql上下文再次注册DbContext以用于框架内注入提交UOW
            services.AddScoped<DbContext>(p => p.GetService<CustomDbContext>());

            //Redis
            services.AddSingleton<IZaabeeRedisClient>(new ZaabeeRedisClient(
                config.GetSection("Redis").Get<string>(),
                TimeSpan.FromSeconds(30), new Serializer()));
            
            //RabbitMQ
            services.AddSingleton<IZaabeeRabbitMqClient>(new ZaabeeRabbitMqClient(
                config.GetSection("RabbitMQ").Get<MqConfig>(),
                new Zaabee.RabbitMQ.NewtonsoftJson.Serializer()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "MicroService", Version = "v1"});
            });
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