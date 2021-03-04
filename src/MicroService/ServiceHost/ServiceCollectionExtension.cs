using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.StackExchangeRedis;
using Zaabee.StackExchangeRedis.Abstractions;
using Zaabee.StackExchangeRedis.MsgPack;

namespace ServiceHost
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services,IConfigurationRoot config)
        {
            //注册EF用于C端仓储层
            services.AddDbContext<CustomDbContext>(options =>
                options.UseNpgsql(config.GetSection("PgSqlPrimary").Get<string>()));
            //使用上面已注册的pgsql上下文再次注册DbContext以用于框架内注入提交UOW
            services.AddScoped<DbContext>(p => p.GetService<CustomDbContext>());
            return services;
        }
        
        public static IServiceCollection AddRedis(this IServiceCollection services,IConfigurationRoot config)
        {
            services.AddSingleton<IZaabeeRedisClient>(new ZaabeeRedisClient(
                config.GetSection("Redis").Get<string>(),
                TimeSpan.FromSeconds(30), new Serializer()));
            return services;
        }
        
        public static IServiceCollection AddRabbitMq(this IServiceCollection services,IConfigurationRoot config)
        {
            services.AddSingleton<IZaabeeRabbitMqClient>(new ZaabeeRabbitMqClient(
                config.GetSection("RabbitMQ").Get<MqConfig>(),
                new Zaabee.RabbitMQ.NewtonsoftJson.Serializer()));
            return services;
        }
    }
}