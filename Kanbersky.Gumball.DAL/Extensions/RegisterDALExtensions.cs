using Kanbersky.Gumball.DAL.Concrete.EntityFramework.Context;
using Kanbersky.Gumball.DAL.Concrete.EntityFramework.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kanbersky.Gumball.DAL.Extensions
{
    public static class RegisterDALExtensions
    {
        public static void RegisterGumballDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<KanberContext>(opt =>
            {
                opt.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
            },
            ServiceLifetime.Singleton);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
