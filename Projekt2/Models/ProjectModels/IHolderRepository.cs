using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projekt2.Models.ProjectModels
{
    public interface IHolderRepository
    {

        Task<Holder> AddHolderAsync(Holder holder);

        Task<Guid> RemoveHolderAsync(Guid holderId);

        Task<List<Holder>> GetAllHoldersAsync(string userId);

        Task<Link> AddLinkAsync(Link link, Guid holderId);

        Task<Guid> RemoveLinkAsync(Guid linkId);

        Task<Link> GetLinkAsync(Guid linkId);

        Task<Guid> ClickAsync(Guid linkId);

        Task<List<Link>> GetPopularLinksAsync();

        Task<Link> ModifyLinkAsync(Link link, Guid help);
    }
}
