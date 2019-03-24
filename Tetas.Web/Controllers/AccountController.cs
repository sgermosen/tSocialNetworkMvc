namespace Tetas.Web.Controllers
{
    using Common.ViewModels;
    using Domain.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper; 
        private readonly IMailHelper _mailHelper;
        //private readonly ICountryRepository countryRepository;
        private readonly IConfiguration _configuration;

        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> userManager;

        // private readonly ILogger<LoginModel> _logger;

        //public AccountController(SignInManager<ApplicationUser> signInManager,
        //UserManager<ApplicationUser> userManager)//, IUserHelper userHelper)
        public AccountController(IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration)
        {
            //_signInManager = signInManager;
            //userManager = userManager;
            // _logger = logger;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _configuration = configuration;
        }



        [TempData]
        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public IActionResult Login()
        {
            //if (!string.IsNullOrEmpty(ErrorMessage))
            //{
            //    ModelState.AddModelError(string.Empty, ErrorMessage);
            //}

            // returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
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

            if (ModelState.IsValid)
            {
                //var result = await _signInManager.PasswordSignInAsync(
                //    login.Email,
                //    login.Password,
                //    login.RememberMe,
                //    false);
                ////, lockoutOnFailure: true
                var result = await _userHelper.LoginAsync(login);

                if (result.Succeeded)
                {
                    // return LocalRedirect(returnUrl);
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
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
            ModelState.AddModelError(string.Empty, "Failed to login.");


            return View(login);
        }

        public async Task<IActionResult> Logout()
        {
            //  await _signInManager.SignOutAsync();
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // var user = await userManager.FindByEmailAsync(model.Email);
                var user = await _userHelper.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        Name = model.FirstName,
                        Lastname = model.LastName,
                        Email = model.Email,
                        UserName = model.Email
                    };

                    //var result = await userManager.CreateAsync(user, model.Password);
                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    //var loginModel = new LoginModel
                    //{
                    //    Password = model.Password,
                    //    RememberMe = false,
                    //    Email = model.Email
                    //};

                    //var result2 = await _signInManager.PasswordSignInAsync(
                    //    model.Email,
                    //    model.Password,
                    //    true,
                    //    false);

                    //var result2 = await _userHelper.LoginAsync(loginModel);
                    //if (result2.Succeeded)
                    //{
                    //    return RedirectToAction("Index", "Home");
                    //}
                    var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    _mailHelper.SendMail(model.Email,
                        "Tetas Email confirmation", $"<h1>Tetas Email confirmation</h1>" +
                                                    $"for the activation of this account " +
                                                    $"please click on the followed link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                    
                  //  ModelState.AddModelError(string.Empty, "The instructions to activate your account was send it to your Email");

                    return RedirectToAction("Index", "Home", new{message= "The instructions to activate your account was send it to your Email" });
                  //  return View(model);
                }

                ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return View(model);
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }



    }
}