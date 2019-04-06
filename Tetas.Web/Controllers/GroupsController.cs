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

    public class GroupsController : PsBaseController
    {
        private readonly IGroup _groupRepository;
       // private readonly IConfiguration _configuration;
      //  private readonly IPsSelectList _psSelectList;

        //  private readonly Repository<Privacy> _privacyRepo;
        // private readonly Repository<GroupType> _groupTypeRepo;
        // private readonly ApplicationDbContext _context;
       
        public GroupsController(IUserHelper userHelper,
            IConfiguration configuration,
            IPsSelectList psSelectList,
            IGroup groupRepository) : base( userHelper, configuration, psSelectList)
        {
            _groupRepository = groupRepository;
          //  _configuration = configuration;
           // _psSelectList = psSelectList;
           // _genericSelectList = new GenericSelectList();
          //  UserHelper = userHelper;
          //  _privacyRepo = new Repository<Privacy>(context);
          // _groupTypeRepo = new Repository<GroupType>(context);
        }

        public async Task<IActionResult> Index()
        {
            var user = await UserHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.MyGroup = _groupRepository.GetGroupWithPosts(user.Id);
            return View();
        }

        public async Task<IActionResult> Details(long? id, GroupTabViewModel vm)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (vm == null)
            {
                vm = new GroupTabViewModel
                {
                    ActiveTab = Tab.GroupPosts
                };
            }

            var group = await _groupRepository.FindByIdAsync(id.Value);

            if (group == null)
            {
                return NotFound();
            }

            vm.Name = group.Name;
            vm.Description = group.Description;
            vm.Id = id.Value;

            return View(vm);
        }

        public IActionResult SwitchToTabs(string tabname, long id)
        {
            var vm = new GroupTabViewModel
            {
                Id = id
            };
            switch (tabname)
            {
                case "Info":
                    vm.ActiveTab = Tab.Info;
                    break;
                case "GroupsMembers":
                    vm.ActiveTab = Tab.GroupMembers;
                    break;
                case "GroupPosts":
                    vm.ActiveTab = Tab.GroupPosts;
                    break;
                default:
                    vm.ActiveTab = Tab.Info;
                    break;
            }
            return RedirectToAction("Details", vm);
        }

        public async Task<IActionResult> Create()
        {
           var groupTypes = await PsSelectList.GetListGroupTypes();

           var privacies =  await PsSelectList.GetListPrivacies();

            var group = new GroupViewModel
            {
                Privacies = GenericSelectList.CreateSelectList(privacies, x => x.Id, x => x.Name),
                GroupTypes = GenericSelectList.CreateSelectList(groupTypes, x => x.Id, x => x.Name)
            };

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await UserHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }

                var group = await MakeGroup(vm, user, true);

                await _groupRepository.AddAsync(group);

                return RedirectToAction(nameof(SwitchToTabs), new { tabname="GroupPosts", group.Id});
            }

            return View(vm);
        }

        public async Task<Group> MakeGroup(GroupViewModel view,
            ApplicationUser user, bool creating)
        {
            var group = new Group
            {
                Name = view.Name,
                Description = view.Description,
                Type = await PsSelectList.GetGroupTypeAsync(view.TypeId),
                Privacy = await PsSelectList.GetPrivacyAsync(view.PrivacyId)
            };

            if (creating)
            {
                group.Link = $"{Configuration["Tokens:UrlBase"]}{view.Name.Replace(" ", "")}";
                group.Owner = user;
                group.CreationDate = DateTime.UtcNow;
            }
            else
            {
                group.Id = view.Id;
                group.Link = view.Link;
            }

            return group;
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group =  await _groupRepository.GetGroupWithPostsAndComments(id.Value);

            if (group == null)
            {
                return NotFound();
            }
                        
            var groupTypes = await PsSelectList.GetListGroupTypes();

            var privacies = await PsSelectList.GetListPrivacies();


            var pList = GenericSelectList.CreateSelectList(privacies, x => x.Id, x => x.Name);
                      
            var vm = new GroupViewModel
            {
                Name = group.Name,
                Description = group.Description,
                Link = group.Link,
                CreationDate = group.CreationDate,
                PictureUrl = group.PictureUrl,
                Type = group.Type,
                Privacy = group.Privacy,
                Owner = group.Owner,
                TypeId = group.Type.Id,
                PrivacyId = group.Privacy.Id,
                Id = group.Id,
                Privacies = pList,
                GroupTypes = GenericSelectList.CreateSelectList(groupTypes, x => x.Id, x => x.Name)
            };
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, GroupViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var user = await CurrentUser(); 
                    if (user == null)
                    {
                        return NotFound();
                    }
                    var group = await MakeGroup(vm, user, false);
                    await _groupRepository.UpdateAsync(group);
                    //vm.Id = group.Id;

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SwitchToTabs), new { tabname = "GroupPosts", group.Id });

            }
            return View(vm);
        }

        public async Task<IActionResult> MyGroups()
        {
            var user = await UserHelper.GetUserByEmailAsync(User.Identity.Name);
            var myGroups = _groupRepository.GetGroupWithPosts(user.Id);
            return PartialView("_MyGroups", myGroups);
        }

        private bool GroupExists(long id)
        {
            return _groupRepository.Exists(id);
        }
    }
}