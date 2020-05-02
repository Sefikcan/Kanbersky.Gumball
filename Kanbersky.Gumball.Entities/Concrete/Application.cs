using Kanbersky.Gumball.Core.Entity;
using Kanbersky.Gumball.Entities.Abstract;
using System.Collections.Generic;

namespace Kanbersky.Gumball.Entities.Concrete
{
    public class Application : BaseEntity,IEntity
    {
        public Application()
        {
            Configs = new HashSet<Config>();
        }

        public string ApplicationName { get; set; }

        public virtual ICollection<Config> Configs { get; set; }
    }
}
