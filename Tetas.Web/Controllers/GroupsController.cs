namespace Tetas.Web.Controllers
{
    using Domain.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Repositories.Contracts;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Tetas.Infraestructure;

    public class GroupsController : PsBaseController
    {
        private readonly IGroup _groupRepository;
        // private readonly IConfiguration _configuration;
        //  private readonly IPsSelectList _psSelectList;

        //  private readonly Repository<Privacy> _privacyRepo;
        // private readonly Repository<GroupType> _groupTypeRepo;
        private readonly ApplicationDbContext _context;

        public GroupsController(IUserHelper userHelper,
            IConfiguration configuration,
            IPsSelectList psSelectList,
            IGroup groupRepository, ApplicationDbContext context) : base(userHelper, configuration, psSelectList)
        {
            _groupRepository = groupRepository;
            _context = context;
            //  _configuration = configuration;
            // _psSelectList = psSelectList;
            // _genericSelectList = new GenericSelectList();
            //  UserHelper = userHelper;
            //  _privacyRepo = new Repository<Privacy>(context);
            // _groupTypeRepo = new Repository<GroupType>(context);
        }


        #region Post
        public async Task<IActionResult> PostInGroup(long id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            var postComment = new GroupPostViewModel
            {
                Group = group,
                GroupId = group.Id
            };
            return View(postComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostInGroup(GroupPostViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await UserHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }
                var group = await _groupRepository.GetByIdAsync(vm.GroupId);
                if (group == null)
                {
                    return NotFound();
                }
                var post = new GroupPost
                {
                    Name = vm.Name,
                    Body = vm.Body,
                    Group = group
                };
                post.Owner = user;
                post.CreationDate = DateTime.UtcNow;

                await _groupRepository.AddPostAsync(post);

                return RedirectToAction(nameof(Details), new { id = group.Id });
            }
            return View(vm);
        }

        //public async Task<IActionResult> EditComment(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var comment = await _groupRepository.get(id.Value);

        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }

        //    if (comment.Owner.Email != User.Identity.Name)
        //    {
        //        return Unauthorized();
        //    }

        //    var postComment = new PostCommentViewModel
        //    {
        //        Id = comment.Id,
        //        PostId = comment.Post.Id,
        //        Body = comment.Body,
        //        Name = comment.Name,
        //        Date = comment.Date,
        //        Owner = comment.Owner,
        //        Post = comment.Post,
        //        OwnerId = comment.Owner.Id
        //    };
        //    return View(postComment);

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditComment(long id, PostCommentViewModel vm)
        //{
        //    if (id != vm.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
        //        if (user == null || user.Id != vm.OwnerId)
        //        {
        //            return NotFound();
        //        }
        //        var post = await _postRepository.GetByIdAsync(vm.PostId);
        //        if (post == null)
        //        {
        //            return NotFound();
        //        }
        //        var comment = new PostComment
        //        {
        //            Id = vm.Id,
        //            Name = vm.Name,
        //            Body = vm.Body,
        //            Owner = user,
        //            Date = vm.Date,
        //            UpdatedDate = DateTime.UtcNow,
        //            Post = post
        //        };

        //        try
        //        {
        //            await _postRepository.UpdateCommentAsync(comment);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await PostCommentExists(comment.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Details), new { id = comment.Post.Id });
        //    }
        //    return View(vm);
        //}

        //public async Task<IActionResult> DeleteComment(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var comment = await _postRepository.GetPostCommentByIdAsync(id.Value);

        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }
        //    if (comment.Owner.Email != User.Identity.Name)
        //    {
        //        return Unauthorized();
        //    }

        //    return View(comment);
        //}

        //[HttpPost, ActionName("DeleteComment")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteCommentConfirmed(long id)
        //{
        //    var comment = await _postRepository.GetPostCommentByIdAsync(id);
        //    var postId = comment.Post.Id;
        //    await _postRepository.DeleteCommentAsync(comment);
        //    return RedirectToAction(nameof(Details), new { id = comment.Post.Id });
        //}

        private async Task<bool> PostExists(long id)
        {
            return await _groupRepository.PostExistAsync(id);
        }
        #endregion              

        #region Group
        public async Task<IActionResult> Index()
        {
            var user = await UserHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.MyGroup = await _groupRepository.GetPublicAndMyGroupsAsync(user.Id);
            return View();
        }

        public async Task<IActionResult> Join(long id, string reason = "")
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            
            var user = await CurrentUser();

            var gMember = await _context.GroupMembers.Where(p => p.Group.Id == id && p.User.Id == user.Id).FirstOrDefaultAsync();

            if (gMember != null && gMember.Banned)
            {
                return Unauthorized();
            }

            if (gMember != null && !gMember.State)
            {
                return RedirectToAction(nameof(SwitchToTabs), new { tabname = "GroupPosts", group.Id });
            }
            
            if (gMember == null)
            {
                if (string.IsNullOrEmpty(reason))
                {
                    reason = "I Want to be Joined to this group";
                }

                gMember = new GroupMember
                {
                    Name = reason,
                    Group = group,
                    ApplicationDate = DateTime.UtcNow,
                    User = user,
                    Banned = false,
                    State = false,
                    Applied = true,
                };

                await _context.GroupMembers.AddAsync(gMember);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Leave(long id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            var user = await CurrentUser();

            var gMember = await _context.GroupMembers.Where(p => p.Group.Id == id && p.User.Id == user.Id).FirstOrDefaultAsync();

            if (gMember != null && gMember.Banned)
            {
                return Unauthorized();
            }

            //if (gMember != null && !gMember.State)
            //{
            //    return RedirectToAction(nameof(SwitchToTabs), new { tabname = "GroupPosts", group.Id });
            //}

            if (gMember != null)
            {
                _context.GroupMembers.Remove(gMember);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
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

            var group = await _groupRepository.GetGroupWithPostsAndComments(id.Value);

            if (group == null)
            {
                return NotFound();
            }

            vm.Name = group.Name;
            vm.Description = group.Description;
            vm.Id = id.Value;
            vm.Owner = group.Owner;

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

            var privacies = await PsSelectList.GetListPrivacies();

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

                return RedirectToAction(nameof(SwitchToTabs), new { tabname = "GroupPosts", group.Id });
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
                group.Owner = view.Owner;
            }

            return group;
        }

        public async Task<IActionResult> Edit(long? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var group = await _groupRepository.GetGroupWithPostsAndComments(id.Value);

            if (group == null)
            {
                return NotFound();
            }

            if (group.Owner.Email != User.Identity.Name)
            {
                return Unauthorized();
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
                return RedirectToAction(nameof(SwitchToTabs), new { tabname = "GroupPosts", vm.Id });

            }
            return View(vm);
        }

        public async Task<IActionResult> MyGroups()
        {
            var user = await UserHelper.GetUserByEmailAsync(User.Identity.Name);
            var myGroups = await _groupRepository.GetPublicAndMyGroupsAsync(user.Id);
            return PartialView("_MyGroups", myGroups);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _groupRepository.GetGroupWithPostsAndComments(id.Value);

            if (group == null)
            {
                return NotFound();
            }
            if (group.Owner.Email != User.Identity.Name)
            {
                return Unauthorized();
            }

            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var group = await _groupRepository.FindByIdAsync(id);
            await _groupRepository.DeleteAsync(group);
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(long id)
        {
            return _groupRepository.Exists(id);
        }
        #endregion

    }
}