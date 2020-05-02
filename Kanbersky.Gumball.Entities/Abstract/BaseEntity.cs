using Kanbersky.Gumball.Core.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kanbersky.Gumball.Entities.Abstract
{
    public class BaseEntity : IEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedAt { get; set; }
    }
}
