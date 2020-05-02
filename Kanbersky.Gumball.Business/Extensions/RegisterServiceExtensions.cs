using AutoMapper;
using Kanbersky.Gumball.Business.Abstract;
using Kanbersky.Gumball.Business.Concrete;
using Kanbersky.Gumball.Business.Mappings.AutoMapper;
using Kanbersky.Gumball.Core.Configs.Abstract;
using Kanbersky.Gumball.Core.Models;
using Kanbersky.Gumball.DAL.Concrete.EntityFramework.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbersky.Gumball.Business.Extensions
{
    public static class RegisterServiceExtensions
    {
        public static void InitializeConfigs(this IApplicationBuilder app, Action<ConfigInitModel> action)
        {
            var configService = app.ApplicationServices.GetRequiredService<IConfigFactory>();
            configService.InitServer(action);
        }

        public static void RegisterGumballServices(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BusinessProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<IApplicationService, ApplicationService>();
        }

        public static async Task LoadConfig(this IApplicationBuilder app, ConfigInitModel configInitModel)
        {
            var configService = app.ApplicationServices.GetRequiredService<IConfigFactory>();
            var dbContext = app.ApplicationServices.GetRequiredService<KanberContext>();
            var configs = await dbContext.Configs
                .Include(x => x.Application)
                .Where(x => x.Application.ApplicationName == configInitModel.RedisClientName)
                .ToListAsync();

            await configService.PublishAsync(configInitModel.RedisClientName,
                configs.ToDictionary(x => x.Key, x => (object)x.Value));
        }
    }
}
