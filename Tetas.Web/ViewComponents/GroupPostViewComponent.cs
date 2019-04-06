using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tetas.Infraestructure;

namespace Tetas.Web.ViewComponents
{
    public class GroupPostViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public GroupPostViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group==null)
            {
                //return NotFound();
            }
            ViewBag.GroupId = id;
            ViewBag.Name = group.Name;

            var gPost = await _context.GroupPosts.Where(p => p.Group.Id == id).ToListAsync();

            return View(gPost);
        }
    }
}
