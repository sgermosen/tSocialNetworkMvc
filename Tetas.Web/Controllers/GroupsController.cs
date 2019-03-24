namespace Tetas.Web.Controllers
{
    using Domain.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Repositories.Contracts;
    using System;
    using System.Threading.Tasks;

    public class GroupsController : Controller
    {
        private readonly IGroup _groupRepository;
        private readonly IUserHelper _userHelper;

        public GroupsController(IGroup groupRepository, IUserHelper userHelper)
        {
            _groupRepository = groupRepository;
            _userHelper = userHelper;
        }
       
        public IActionResult Index()
        {
            ViewBag.MyGroup = _groupRepository.GetGroupWithPosts(User.Identity.Name);
            return View();
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _groupRepository.FindByIdAsync(id.Value);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group group)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }
                group.Owner = user;
                group.CreationDate = DateTime.UtcNow;

                await _groupRepository.AddAsync(group);

                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _groupRepository.FindByIdAsync(id.Value);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    await _groupRepository.UpdateAsync(group);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }



        private bool GroupExists(long id)
        {
            return _groupRepository.Exists(id);
        }
    }
}