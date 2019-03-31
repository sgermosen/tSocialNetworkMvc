using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tetas.Infraestructure;

namespace Tetas.Web.ViewComponents
{
    public class GroupMemberViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public GroupMemberViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var gMembers = await _context.GroupMembers.ToListAsync();

            return View(gMembers);
        }
    }
}
