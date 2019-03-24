using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Tetas.Front.Models;

namespace Tetas.Front.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.AspNet.Identity;
    using Domain;

    public class BaseController : Controller
    {
        protected LocalDataContext Db;

        protected virtual void Seed()
        {

        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //if(Request.IsAjaxRequest)
            base.OnException(filterContext);
        }

        public BaseController()
        {
            Db = new LocalDataContext();
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        public async Task<string> GetUserId()
        {
            //if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) != 0) return Convert.ToInt32(Session["UserId"]);
            //var manager =
            //    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
           // var currentUser = manager.FindById(User.Identity.GetUserId());
            // Session["UserId"] =
            return User.Identity.GetUserId(); // await UsersHelper.GetUserId(currentUser.Email);
            //return Convert.ToInt32(Session["UserId"]);
        }
        public async Task<string> GetUserIdByEmail(string email)
        {
             var manager =
                 new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
       
            try
            {
                var currentUser = await manager.FindByEmailAsync(email);
                return currentUser.Id;
            }
            catch (Exception )
            {
                return "";
            }
           // await UsersHelper.GetUserId(currentUser.Email);
            //return Convert.ToInt32(Session["UserId"]);
        }

        public async Task<string> GetUserIdByUnivId(string univId)
        {
            var user = await Db.Users.Where(p => p.UniversityId == univId).FirstOrDefaultAsync();
            return user == null ? "" : user.UserId;
            //return Convert.ToInt32(Session["UserId"]);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}