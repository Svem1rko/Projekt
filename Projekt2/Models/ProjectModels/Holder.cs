using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projekt2.Models.ProjectModels
{
    public class Holder
    {
        [MaxLength(40), MinLength(1), Required]
        public string Name { get; set; }
        public string UserId { get; set; }
        public Guid Id { get; set; }
        public List<Link> Links { get; set; }

        public Holder()
        {
            Id = Guid.NewGuid();
        }

        public Holder(string name, string userId)
        {
            Name = name;
            UserId = userId;
            Id = Guid.NewGuid();
            Links = new List<Link>();
        }

        public Holder(Guid id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Holder holder)
            {
                return holder.Id.Equals(Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
