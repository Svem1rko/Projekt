using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projekt2.Models;
using Projekt2.Models.ProjectModels;

namespace Projekt2.Controllers
{
    
    public class HolderController : Controller
    {
        private readonly IHolderRepository _repository;

        private readonly UserManager<ApplicationUser> _user;

        public HolderController(IHolderRepository repository, UserManager<ApplicationUser> user)
        {
            _repository = repository;
            _user = user;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _user.GetUserAsync(HttpContext.User);

            if (currentUser == null)
            {
                return RedirectToAction("NotLogged");
            }

            await RemoveHelp();
            var all = await _repository.GetAllHoldersAsync(currentUser.Id);
            var popular = await _repository.GetPopularLinksAsync();

            var vm = new IndexViewModel(all, popular);

            return View(vm);
        }
        [Authorize]
        public IActionResult AddHolder()
        {
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> AddHolder(Holder holder)
        {
            if (ModelState.IsValid)
            {
                var user = await _user.GetUserAsync(HttpContext.User);
                holder.UserId = user.Id;

                await _repository.AddHolderAsync(holder);
                return RedirectToAction("Index");
            }

            return View(holder);
        }
        [Authorize]
        public IActionResult AddLink(Guid holderId)
        {
            var helpHolder = new Holder
            {
                Name = holderId.ToString(),
                UserId = "helper",
                Id = Guid.NewGuid()
            };
            _repository.AddHolderAsync(helpHolder);
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> AddLink(Link link)
        {
            if (ModelState.IsValid)
            {
                var helpHolder =await  _repository.GetAllHoldersAsync("helper");
                var holder = helpHolder.First(h => h.UserId.Equals("helper"));

                if (link.Description == null)
                    link.Description = "-";

                await _repository.AddLinkAsync(link, Guid.Parse(holder.Name));
                await _repository.RemoveHolderAsync(holder.Id);

                return RedirectToAction("Index");
            }

            return View(link);
        }

        [Authorize]
        public async Task<IActionResult> RemoveHolder(Guid holderId)
        {
            await _repository.RemoveHolderAsync(holderId);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> RemoveLink(Guid linkId)
        {
            await _repository.RemoveLinkAsync(linkId);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> LinkInfo(Guid linkId)
        {
            var link = await _repository.GetLinkAsync(linkId);
            return View(link);
        }

        [Authorize]
        public async Task<IActionResult> Click(Guid linkId, string url)
        {
            await _repository.ClickAsync(linkId);
            return Redirect(url);
        }

        [Authorize]
        public async Task<IActionResult> Back()
        {
            await RemoveHelp();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Error()
        {
            return View();
        }
        
        public IActionResult NotLogged()
        {
            return View();
        }

        [Authorize]
        public IActionResult Modify(Guid linkId)
        {
            var helpHolder = new Holder
            {
                Name = linkId.ToString(),
                UserId = "helper",
                Id = Guid.NewGuid()
            };
            _repository.AddHolderAsync(helpHolder);
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Modify(Link link)
        {
            if (link.Name != null || link.Description != null)
            {
                var helpHolder = await _repository.GetAllHoldersAsync("helper");
                var holder = helpHolder.First(h => h.UserId.Equals("helper"));
                
                await _repository.ModifyLinkAsync(link, Guid.Parse(holder.Name));
                await _repository.RemoveHolderAsync(holder.Id);

                return RedirectToAction("Index");
            }

            return View(link);
        }

        [Authorize]
        public async Task<int> RemoveHelp()
        {
            var help = await _repository.GetAllHoldersAsync("helper");
            foreach (var holder in help)
            {
                await _repository.RemoveHolderAsync(holder.Id);
            }

            return 0;
        }
    }
}