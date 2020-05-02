using Kanbersky.Gumball.Core.Entity;
using Kanbersky.Gumball.Entities.Abstract;

namespace Kanbersky.Gumball.Entities.Concrete
{
    public class Config : BaseEntity,IEntity
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public int ApplicationId { get; set; }

        public virtual Application Application { get; set; }
    }
}
