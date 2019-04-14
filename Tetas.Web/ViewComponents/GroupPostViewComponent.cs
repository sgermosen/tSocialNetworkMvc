using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tetas.Domain.Entities;
using Tetas.Infraestructure;
using Tetas.Repositories.Contracts;
using Tetas.Web.Helpers;
using Tetas.Web.Models;

namespace Tetas.Web.ViewComponents
{
    public class GroupPostViewComponent : ViewComponent
    {
        private IUserHelper UserHelper;
        private IConfiguration Configuration;
        private IPsSelectList PsSelectList;
        private GenericSelectList GenericSelectList;
        private readonly ApplicationDbContext _context;

        public GroupPostViewComponent(IUserHelper userHelper, IConfiguration configuration, IPsSelectList psSelectList, ApplicationDbContext context)            
        {
            UserHelper = userHelper;
            Configuration = configuration;
            PsSelectList = psSelectList;
            _context = context;
            GenericSelectList = new GenericSelectList();
        }

        public async Task<ApplicationUser> CurrentUser()
        {
            return await UserHelper.GetUserByEmailAsync(User.Identity.Name);
        }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            var group = await _context.Groups.Include(o=>o.Owner).Where(p=>p.Id==id).FirstOrDefaultAsync();

            if (group==null)
            {
                //return NotFound();
            }
            ViewBag.GroupId = id;
            ViewBag.Name = group.Name;

            ViewBag.IsAdmin = false;
            ViewBag.IsMember = false;
            ViewBag.IsBanned = false;

            var user = await CurrentUser();

            if(group.Owner.Id==user.Id)
            {
                ViewBag.IsAdmin = true;
            }

            var gPost = new List<GroupPost>();
                       
            var gMember = await _context.GroupMembers
                .Where(p => p.Group.Id == id && p.User.Id == user.Id)
                .FirstOrDefaultAsync();

            if(gMember!=null)
            {
                ViewBag.IsMember = gMember.State;
                ViewBag.IsBanned = gMember.Banned;
                gPost = await _context.GroupPosts.Where(p => p.Group.Id == id).ToListAsync();
            }                

            return View(gPost);
        }
    }
}
