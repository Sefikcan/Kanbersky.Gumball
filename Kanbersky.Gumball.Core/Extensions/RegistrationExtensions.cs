using Kanbersky.Gumball.Core.Caching.Abstract;
using Kanbersky.Gumball.Core.Caching.Concrete.MemCache;
using Kanbersky.Gumball.Core.Caching.Concrete.Redis;
using Kanbersky.Gumball.Core.Configs.Abstract;
using Kanbersky.Gumball.Core.Configs.Concrete;
using Kanbersky.Gumball.Core.Middlewares;
using Kanbersky.Gumball.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kanbersky.Gumball.Core.Extensions
{
    public static class RegistrationExtensions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void RegisterGumballPipelines(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfigSettings>(options => configuration.GetSection("ConfigSettings").Bind(options));
            services.Configure<ElasticSearchSettings>(options => configuration.GetSection("ElasticSearchSettings").Bind(options));

            services.AddSingleton<IConfigFactory, ConfigFactory>();

            //register cache
            //services.AddScoped<ICacheService, MemoryCacheService>();
            //services.AddMemoryCache();

            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
            });
        }
    }
}
