using System.Data.Entity;

namespace Projekt2.Models.ProjectModels
{
    public class HolderDbContext : DbContext
    {
        public IDbSet<Holder> Holders { get; set; }
        public IDbSet<Link> Links { get; set; }

        public HolderDbContext(string cnnstr) : base(cnnstr) {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Holder>().HasKey(h => h.Id);
            modelBuilder.Entity<Holder>().Property(h => h.UserId).IsRequired();
            modelBuilder.Entity<Holder>().Property(h => h.Name).IsRequired();
            modelBuilder.Entity<Holder>().HasMany(h => h.Links);

            modelBuilder.Entity<Link>().HasKey(l => l.Id);
            modelBuilder.Entity<Link>().Property(l => l.Url).IsRequired();
            modelBuilder.Entity<Link>().Property(l => l.Name).IsRequired();
        }
    }
}
