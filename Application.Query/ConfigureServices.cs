using System.Reflection;
using Application.Query.Common.Models;
using Application.Query.Infrastructure.Persistance;
using Application.Shared.Models;
using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Application.Query;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationQuery(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
        services.AddSingleton<IMongoDbSettings>((sp) => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        services.AddSingleton<ReadDbContext>();
        services.AddSingleton<IMongoClient>((sp) => new MongoClient(sp.GetRequiredService<IMongoDbSettings>().ConnectionString));
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                cfg.Host(new Uri(rabbitMqSettings.Host), h =>
                {
                    h.Username(rabbitMqSettings.UserName);
                    h.Password(rabbitMqSettings.Password);
                });
            });

            x.AddConsumers(typeof(ApplicationQueryRoot).Assembly);
        });

        services.AddFastEndpoints();
        services.SwaggerDocument();

        return services;
    }
}
