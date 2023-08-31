using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.models;
using Application.Infrastructure.Interceptors;
using Application.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Argument.IsNotNull(configuration.GetConnectionString("DefaultConnection"));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        services.Configure<MongoDbSettings>((op) => configuration.GetSection(nameof(MongoDbSettings)));
        services.AddSingleton<IMongoDbSettings>((sp) => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        services.AddSingleton<ReadDbContext>();
        services.AddSingleton<IMongoClient>((sp) => new MongoClient(sp.GetRequiredService<IMongoDbSettings>().ConnectionString));

        return services;
    }
}
