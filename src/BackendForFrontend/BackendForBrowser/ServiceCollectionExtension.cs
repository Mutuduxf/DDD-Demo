using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using QueryService;
using Zaabee.StackExchangeRedis;
using Zaabee.StackExchangeRedis.Abstractions;
using Zaabee.StackExchangeRedis.MsgPack;

namespace BackendForBrowser
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQueryService(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<UserQueryService>()
                .AddClasses(classes =>
                    classes.Where(@class => @class.Name.EndsWith("QueryService", StringComparison.OrdinalIgnoreCase)))
                .AsSelf().WithScopedLifetime());
            return services;
        }

        public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfigurationRoot config)
        {
            services.AddScoped<IDbConnection>(
                _ => new NpgsqlConnection(config.GetSection("PgSqlStandby").Get<string>()));
            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfigurationRoot config)
        {
            services.AddSingleton<IZaabeeRedisClient>(new ZaabeeRedisClient(
                config.GetSection("Redis").Get<string>(),
                TimeSpan.FromSeconds(30), new Serializer()));
            return services;
        }
    }
}