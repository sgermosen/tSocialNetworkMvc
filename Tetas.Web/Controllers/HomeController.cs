using Microsoft.EntityFrameworkCore;

namespace Tetas.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Repositories.Contracts;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly IPost _postRepository;
        private readonly IGroup _groupRepository;

        public HomeController(IPost postRepository,IGroup groupRepository)
        {
            _postRepository = postRepository;
            _groupRepository = groupRepository;
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
            var model =   _groupRepository.GetGroups("");

            model = model.OrderByDescending(p => p.CreationDate).Take(10);

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
