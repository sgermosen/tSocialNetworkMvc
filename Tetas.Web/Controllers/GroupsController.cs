namespace Tetas.Web.Controllers
{
    using Domain.Entities;
    using Helpers;
    using Infraestructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Repositories.Contracts;
    using Repositories.Implementations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GroupsController : Controller
    {
        private readonly IGroup _groupRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly GenericSelectList _genericSelectList;
        private readonly Repository<Privacy> _privacyRepo;
        private readonly Repository<GroupType> _groupTypeRepo;
        // private readonly ApplicationDbContext _context;

        public GroupsController(IGroup groupRepository,
            IUserHelper userHelper,
            ApplicationDbContext context, IConfiguration configuration)
        {
            _groupRepository = groupRepository;
            _userHelper = userHelper;
            _configuration = configuration;
            _genericSelectList = new GenericSelectList();
            _privacyRepo = new Repository<Privacy>(context);
            _groupTypeRepo = new Repository<GroupType>(context);
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.MyGroup = _groupRepository.GetGroupWithPosts(user.Id);
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
            var groupTypes = new List<GroupType>
            {
                new GroupType
                {
                    Id = 0,
                    Name = "Choose One Group Type"
                }
            };

            var privacies = new List<Privacy>
            {
                new Privacy
                {
                    Id = 0,
                    Name = "Choose One Privacy Level"
                }
            };

            groupTypes.AddRange(_groupTypeRepo.GetAll().ToList());

            var group = new GroupViewModel
            {
                Privacies = _genericSelectList.CreateSelectList(privacies, x => x.Id, x => x.Name),
                GroupTypes = _genericSelectList.CreateSelectList(groupTypes, x => x.Id, x => x.Name)
            };

            privacies.AddRange(_privacyRepo.GetAll().ToList());

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupViewModel view)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }

                var group = await MakeGroup(view, user, true);

                await _groupRepository.AddAsync(@group);

                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        public async Task<Group> MakeGroup(GroupViewModel view,
            ApplicationUser user, bool creating)
        {
            var groupType = await _groupTypeRepo.GetByIdAsync(view.TypeId);

            var privacy = await _privacyRepo.GetByIdAsync(view.PrivacyId);

            var group = new Group
            {
                Name = view.Name,
                Description = view.Description,
                Type = groupType,
                Privacy = privacy
            };

            if (creating)
            {
                group.Link = $"{_configuration["Tokens:UrlBase"]}{view.Name.Replace(" ", "")}";
                group.Owner = user;
                group.CreationDate = DateTime.UtcNow;
            };

            return group;
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

        public async Task<IActionResult> MyGroups()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var myGroups = _groupRepository.GetGroupWithPosts(user.Id);
            return PartialView("_MyGroups", myGroups);
        }

        private bool GroupExists(long id)
        {
            return _groupRepository.Exists(id);
        }
    }
}