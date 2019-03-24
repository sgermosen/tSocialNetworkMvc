using System;

namespace Tetas.Front.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Models;
    using Tools;
    using Domain.Repositories;
    using Domain.DataEntities;

    public class PeopleController : BaseController
    {
        private UserRepository _repository;

        private readonly ApplicationSignInManager _signInManager;

        private readonly ApplicationUserManager _userManager;

        protected override void Seed()
        {
            _repository = new UserRepository(Db);
            base.Seed();
        }
        public PeopleController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public PeopleController()
        {
            _repository = new UserRepository(Db);
        }

        public ApplicationUserManager UserManager => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ApplicationSignInManager SignInManager => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var people = await _repository.FindByClause();

            return View(people);
        }

        [Authorize]
        public async Task<ActionResult> Details(int id)
        {
            var person = await _repository.FindById(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(Db.Countries, "CountryId", "Name");
            ViewBag.GenderId = new SelectList(Db.Genders, "GenderId", "Name");
            ViewBag.StatusId = new SelectList(Db.Status, "StatusId", "Name");
            ViewBag.UserTypeId = new SelectList(Db.UserTypes, "UserTypeId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView userView)
        {
            userView.BornDate = DateTime.Now;
            userView.UserTypeId = 1;
            userView.StatusId = 1;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(userView.Email))
                {
                    ModelState.AddModelError(string.Empty, "You must register with an Email");
                }
                var userId = await GetUserIdByEmail(userView.Email);
                if (!string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError(string.Empty, "There is already an account with this Email, please try to login");
                }
                if (string.IsNullOrEmpty(userView.UniversityId))
                {
                    userId = await GetUserIdByUnivId(userView.UniversityId);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        ModelState.AddModelError(string.Empty, "There is already an account with this University ID, please try to login");
                    }
                }

                var pic = string.Empty;
                const string folder = "~/Content/UserPics";

                if (userView.ImageFile != null)
                {
                    pic = Files.UploadPhoto(userView.ImageFile, folder, "");
                    pic = $"{folder}/{pic}";
                }

                var userAsp = new ApplicationUser { UserName = userView.Email, Email = userView.Email };
                var result = await UserManager.CreateAsync(userAsp, userView.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(userAsp, false, false);

                    var user = new User
                    {
                        UserId = userAsp.Id,
                        StatusId = userView.StatusId,
                        UserTypeId = userView.UserTypeId,
                        UniversityId = userView.UniversityId,
                        Name = userView.Name,
                        LastName = userView.LastName,
                        BornDate = userView.BornDate,
                        CountryId = userView.CountryId,
                        GenderId = userView.GenderId,
                        Picture = pic
                    };

                    await _repository.AddAsync(user);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);

                // return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(Db.Countries, "CountryId", "Name", userView.CountryId);
            ViewBag.GenderId = new SelectList(Db.Genders, "GenderId", "Name", userView.GenderId);
            ViewBag.StatusId = new SelectList(Db.Status, "StatusId", "Name", userView.GenderId);
            ViewBag.UserTypeId = new SelectList(Db.UserTypes, "UserTypeId", "Name", userView.GenderId);

            return View(userView);

        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var person = await _repository.FindById(id);

            if (person == null)
            {
                return HttpNotFound();
            }

            ViewBag.CountryId = new SelectList(Db.Countries, "CountryId", "Name", person.CountryId);
            ViewBag.GenderId = new SelectList(Db.Genders, "GenderId", "Name", person.GenderId);
            ViewBag.UserId = new SelectList(Db.Users, "UserId", "Email", person.UserId);

            return View(person);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User person)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(person);

                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(Db.Countries, "CountryId", "Name", person.CountryId);
            ViewBag.GenderId = new SelectList(Db.Genders, "GenderId", "Name", person.GenderId);
            ViewBag.UserId = new SelectList(Db.Users, "UserId", "Email", person.UserId);

            return View(person);
        }
    }
}
