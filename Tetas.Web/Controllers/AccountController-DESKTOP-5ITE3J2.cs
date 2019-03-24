namespace Tetas.Web.Controllers
{
    using Common.ViewModels;
    using Domain.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> userManager;

        // private readonly ILogger<LoginModel> _logger;

        //public AccountController(SignInManager<ApplicationUser> signInManager,
        //UserManager<ApplicationUser> userManager)//, IUserHelper userHelper)
        public AccountController(IUserHelper userHelper)
        {
            //_signInManager = signInManager;
            //this.userManager = userManager;
            // _logger = logger;
            this._userHelper = userHelper;
        }



        [TempData]
        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

 
        public async Task<IActionResult> Login()
        {
            //if (!string.IsNullOrEmpty(ErrorMessage))
            //{
            //    ModelState.AddModelError(string.Empty, ErrorMessage);
            //}

            // returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }
            // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login)
        {
            // var returnUrl = "Home/Index";// returnUrl ?? Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                //var result = await _signInManager.PasswordSignInAsync(
                //    login.Email,
                //    login.Password,
                //    login.RememberMe,
                //    false);
                ////, lockoutOnFailure: true
                var result = await this._userHelper.LoginAsync(login);

                if (result.Succeeded)
                {
                    // return LocalRedirect(returnUrl);
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = login.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    // _logger.LogWarning("User account locked out.");
                //    return RedirectToPage("./Lockout");
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    return View(login);
                //}

            }
            this.ModelState.AddModelError(string.Empty, "Failed to login.");


            return View(login);
        }

        public async Task<IActionResult> Logout()
        {
          //  await _signInManager.SignOutAsync();
             await this._userHelper.LogoutAsync();
               return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (this.ModelState.IsValid)
            {
               // var user = await this.userManager.FindByEmailAsync(model.Email);
                  var user = await this._userHelper.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        Name = model.FirstName,
                        Lastname = model.LastName,
                        Email = model.Email,
                        UserName = model.Email
                    };
 
                    //var result = await this.userManager.CreateAsync(user, model.Password);
                      var result = await this._userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return this.View(model);
                    }

             var loginModel = new LoginModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Email = model.Email
                    };

                    //var result2 = await _signInManager.PasswordSignInAsync(
                    //    model.Email,
                    //    model.Password,
                    //    true,
                    //    false);

                      var result2 = await this._userHelper.LoginAsync(loginModel);
                    if (result2.Succeeded)
                    {
                        return this.RedirectToAction("Index", "Home");
                    }

                    this.ModelState.AddModelError(string.Empty, "The user couldn't be login.");
                    return this.View(model);
                }

                this.ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return this.View(model);
        }


    }
}