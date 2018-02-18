using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt2.Models.ProjectModels
{
    public class HolderRepository : IHolderRepository
    {
        private readonly HolderDbContext _context;

        public HolderRepository(HolderDbContext context)
        {
            _context = context;
        }
        
        public async Task<Holder> AddHolderAsync(Holder holder)
        {
            _context.Holders.Add(holder);
            await _context.SaveChangesAsync();
            return holder;
        }

        public async Task<Guid> RemoveHolderAsync(Guid holderId)
        {
            var holder = await _context.Holders.Include(h => h.Links).FirstAsync(h => h.Id == holderId);

            for (var i = 0; i < holder.Links.Capacity; i++)
            {
                var link = holder.Links.FirstOrDefault();
                if (link != null)
                    _context.Links.Remove(link);
            }

            _context.Holders.Remove(holder);
            await _context.SaveChangesAsync();
            return holderId;
        }
        
        public Task<List<Holder>> GetAllHoldersAsync(string userId)
        {
            return _context.Holders.Include(h => h.Links).Where(h => h.UserId == userId).ToListAsync();
        }

        public async Task<Link> AddLinkAsync(Link link, Guid holderId)
        {
            var holder = await _context.Holders.Include(h => h.Links).FirstAsync(h => h.Id == holderId);
            holder.Links.Add(link);
            _context.Entry(holder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return link;
        }
        
        public async Task<Guid> RemoveLinkAsync(Guid linkId)
        {
            var link = await _context.Links.FirstAsync(l => l.Id == linkId);
            _context.Links.Remove(link);
            await _context.SaveChangesAsync();
            return linkId;
        }
        
        public Task<Link> GetLinkAsync(Guid linkId)
        {
            return _context.Links.FirstOrDefaultAsync(l => l.Id == linkId);
        }

        public async Task<Guid> ClickAsync(Guid linkId)
        {
            var link = await _context.Links.FirstOrDefaultAsync(l => l.Id.Equals(linkId));
            if (link != null)
                link.Clicked++;
            await _context.SaveChangesAsync();
            return linkId;
        }

        public async Task<List<Link>> GetPopularLinksAsync()
        {
            var links = await _context.Links.OrderBy(l => l.Url).ToListAsync();
            var popular = new LinkedList<Link>();
            Link helpLink = null;

            foreach (var link in links)
            {
                if (helpLink == null)
                {
                    helpLink = link;
                }
                else if (link.Url.Equals(helpLink.Url))
                {
                    helpLink.Clicked += link.Clicked;
                }
                else
                {
                    popular.AddFirst(helpLink);
                    helpLink = link;
                }
            }
            if (helpLink != null)
            {
                popular.AddFirst(helpLink);
                return popular.OrderByDescending(l => l.Clicked).Take(5).ToList();
            }
            return new List<Link>(popular);
        }

        public async Task<Link> ModifyLinkAsync(Link link, Guid help)
        {
            var old = await _context.Links.FirstAsync(l => l.Id == help);

            if (link.Name != null)
                old.Name = link.Name;
            if (link.Description != null)
                old.Description = link.Description;

            await _context.SaveChangesAsync();
            return link;
        }
    }
}
