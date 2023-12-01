using Caching.SimpleInfra.Application.Common.Identity.Services;
using Caching.SimpleInfra.Persistence.Caching.Brokers;
using Caching.SimpleInfra.Persistence.DataContexts;
using Caching.SimpleInfra.Persistence.Repositories;
using Caching.SimpleInfra.Persistence.Repositories.Interfaces;
using LocalIdentity.SimpleInfra.Api.Data;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Caching.Brokers;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Api.Configurations;

public static partial class HostConfiguration
{
    private static WebApplicationBuilder AddCaching(this WebApplicationBuilder builder)
    {
        // register cache settings
        builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(nameof(CacheSettings)));

        // register lazy memory cache
        builder.Services.AddLazyCache();
        
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
            options.InstanceName = "Caching.SimpleInfra";
        });

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddSingleton<ICacheBroker, LazyMemoryCacheBroker>();
        builder.Services.AddSingleton<ICacheBroker, RedisDistributedCacheBroker>();

        return builder;
    }

    private static WebApplicationBuilder AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        // register db contexts
        builder.Services.AddDbContext<IdentityDbContext>(
            options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // register repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        // register foundation data access services
        builder.Services.AddScoped<IUserService, UserService>();

        return builder;
    }

    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();

        return builder;
    }

    private static async ValueTask<WebApplication> SeedDataAsync(this WebApplication app)
    {
        var serviceScope = app.Services.CreateScope();
        await serviceScope.ServiceProvider.InitializeSeedAsync();

        return app;
    }

    private static WebApplication UseExposers(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }
}