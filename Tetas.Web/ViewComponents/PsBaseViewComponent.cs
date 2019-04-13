using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tetas.Domain.Entities;
using Tetas.Infraestructure;
using Tetas.Repositories.Contracts;
using Tetas.Web.Helpers;

namespace Tetas.Web.ViewComponents
{
    public class PsBaseViewComponent: ViewComponent
    {
        protected IUserHelper UserHelper;
        protected IConfiguration Configuration;
        protected IPsSelectList PsSelectList;
        protected GenericSelectList GenericSelectList;
        public readonly ApplicationDbContext _context;

        public PsBaseViewComponent(IUserHelper userHelper,
            IConfiguration configuration,
            IPsSelectList psSelectList, ApplicationDbContext context)
        {
            UserHelper = userHelper;
            Configuration = configuration;
            PsSelectList = psSelectList;
            _context = context;
            GenericSelectList = new GenericSelectList();
        }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            return View();
        }
        public async Task<ApplicationUser> CurrentUser()
        {
            return await UserHelper.GetUserByEmailAsync(User.Identity.Name);
        }
    }
}
