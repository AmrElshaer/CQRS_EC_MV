using System.Reflection;
using Application.Command.Common.Behaviours;
using Application.Command.Infrastructure.Interceptors;
using Application.Command.Infrastructure.Persistence;
using Application.Shared.Models;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Application.Command;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationCommand(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Argument.IsNotNull(configuration.GetConnectionString("DefaultConnection"));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<ISaveChangesInterceptor, DispatchEventsInterceptor>();
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));

        services.AddDbContext<WriteDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

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
        });

        services.AddCap(capOptions =>
        {
            capOptions.UseSqlServer(options =>
            {
                options.ConnectionString = connectionString;
                options.Schema = "outbox";
            });

            capOptions.UseInMemoryMessageQueue();

            capOptions.UseDashboard(dashboardOptions =>
            {
                dashboardOptions.PathMatch = "/cap";
            });
        });

        services.AddSingleton<DapperDbContext>((sp) => new DapperDbContext(connectionString));
        services.AddFastEndpoints();
        services.SwaggerDocument();

        return services;
    }
}
