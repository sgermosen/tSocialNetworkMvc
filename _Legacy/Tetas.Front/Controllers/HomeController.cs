namespace Tetas.Front.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Domain.Repositories;

    public class HomeController : BaseController
    {
        private readonly PostRepository _repository;
        private readonly GroupRepository _gRepository;

        public HomeController()
        {
            _repository = new PostRepository(Db);
            _gRepository = new GroupRepository(Db);
        }        
        protected override void Seed()
        {
          
            base.Seed();
        }

        public async Task<ActionResult> Index()
        {
            var userId = await GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userPosts = await _repository.FindByClause();//.OrderByDescending(p => p.PostDate);

            ViewBag.ConectedUserId = userId;

            //loading groups
            var userGroups = await _gRepository.FindByClause(p => p.UserId == userId);

            userGroups = userGroups.OrderByDescending(p => p.Name).ToList();

            ViewBag.UserGroups = userGroups;
            //  RenderRazorViewToString("_GroupsListPartial",  userGroups);

            return View(userPosts);
        }

        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                    viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                    ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public async Task<ActionResult> GroupList(string id)
        {
            var userId = "";
            if (string.IsNullOrEmpty(id))
            {
                userId = await GetUserId();
            }
            else
            {
                userId = id;
            }
                   
            var userGroups = await _gRepository.FindByClause(p => p.UserId == userId);
            userGroups = userGroups.OrderByDescending(p => p.Name).ToList();
            return View( userGroups);
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}