using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt2.Models.ProjectModels
{
    public class Link
    {
        public Guid Id { get; set; }

        [Required]
        public string Url { get; set; }

        [MinLength(1), MaxLength(30), Required]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; } 

        public int Clicked { get; set; }

        public Link()
        {
            Id = Guid.NewGuid();
        }

        public Link(string name, string url, string description)
        {
            Name = name;
            Url = url;
            Description = description;
            Id = Guid.NewGuid();
            Clicked = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Link link)
            {
                return link.Id.Equals(Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
