namespace Tetas.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    using Tetas.Domain.Entities;
    using Tetas.Repositories.Contracts;
    using Tetas.Web.Helpers;

    public class PsBaseController : Controller
    {
        protected IUserHelper UserHelper;
        protected IConfiguration Configuration;
        protected IPsSelectList PsSelectList;
        protected GenericSelectList GenericSelectList;
        
        public PsBaseController(IUserHelper userHelper,
            IConfiguration configuration,
            IPsSelectList psSelectList)
        {
            UserHelper = userHelper;
            Configuration = configuration;
            PsSelectList = psSelectList;
            GenericSelectList = new GenericSelectList();
        }

        public async Task<ApplicationUser> CurrentUser()
        {
            return await UserHelper.GetUserByEmailAsync(User.Identity.Name);
        }
    }
}