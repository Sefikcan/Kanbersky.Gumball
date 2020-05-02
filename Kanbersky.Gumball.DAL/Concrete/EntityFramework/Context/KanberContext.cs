using Kanbersky.Gumball.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Kanbersky.Gumball.DAL.Concrete.EntityFramework.Context
{
    public class KanberContext : DbContext
    {
        public KanberContext(DbContextOptions<KanberContext> options) : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Config> Configs { get; set; }
    }
}
