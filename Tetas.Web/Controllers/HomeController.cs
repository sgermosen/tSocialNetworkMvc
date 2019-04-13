using Microsoft.EntityFrameworkCore;

namespace Tetas.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Repositories.Contracts;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Tetas.Web.Helpers;

    public class HomeController : Controller
    {
        private readonly IPost _postRepository;
        private readonly IGroup _groupRepository;
        private readonly IUserHelper userHelper;

        public HomeController(IPost postRepository,IGroup groupRepository, 
            IUserHelper userHelper)
        {
            _postRepository = postRepository;
            _groupRepository = groupRepository;
            this.userHelper = userHelper;
        }

        public IActionResult Index(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult PostFilter()
        {
            var model = _postRepository.GetPostWithComments("");

            model = model.OrderByDescending(p => p.Date).Take(10);

            //var modelRet = await model.ToListAsync();
            //var result = RenderRazorViewToString("_NewsPartial", await model.ToListAsync());
            //return Json(new { Table = result }, JsonRequestBehavior.AllowGet);
            //ViewBag.Email = User.FindFirst(ClaimTypes.Email).Value;
            return PartialView("_NewsPartial", model);
        }

        public ActionResult GroupsFilter()
        {
            //var user = Task.Run(userHelper.GetUserByEmailAsync(User.Identity.Name));

            var model =   _groupRepository.GetPublicAndMyGroupsAsync("af581e6a-e402-4b6e-b03a-69220e2c1e2e");
            //model = model.OrderByDescending(p => p.CreationDate);


            //model = model.Take(10);

            return PartialView("_GroupsPartial",  model);
        }

        //[HttpPost]
        //public IActionResult Students(StudentFilter filters)
        //{
        //    List<> students = Student.GetStudents(filters);
        //    return PartialView("_Students", students);
        //}
        // }
        //protected string RenderRazorViewToString(string viewName, object model)
        //    {
        //        ViewData.Model = model;
        //        using (var sw = new StringWriter())
        //        {
        //            var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
        //                viewName);
        //            var viewContext = new ViewContext(ControllerContext, viewResult.View,
        //                ViewData, TempData, sw);
        //            viewResult.View.Render(viewContext, sw);
        //            viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
        //            return sw.GetStringBuilder().ToString();
        //        }
        //    }

    }
}
