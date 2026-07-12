namespace Tetas.Web.Controllers
{
    using Common.ViewModels;
    using Domain.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        //private readonly ICountryRepository countryRepository;
        private readonly IConfiguration _configuration;
        private readonly Repositories.Contracts.IModeration _moderation;

        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> userManager;

        // private readonly ILogger<LoginModel> _logger;

        //public AccountController(SignInManager<ApplicationUser> signInManager,
        //UserManager<ApplicationUser> userManager)//, IUserHelper userHelper)
        public AccountController(IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration,
            Repositories.Contracts.IModeration moderation)
        {
            //_signInManager = signInManager;
            //userManager = userManager;
            // _logger = logger;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _configuration = configuration;
            _moderation = moderation;
        }



        [TempData]
        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        [AllowAnonymous]
        public IActionResult Login(string message)
        {
            //if (!string.IsNullOrEmpty(ErrorMessage))
            //{
            //    ModelState.AddModelError(string.Empty, ErrorMessage);
            //}

            // returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewBag.Message = message;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
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
                        UserName = model.Email,
                        NickName = model.NickName,
                        PhoneNumber = model.Phone
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

                    try
                    {
                        _mailHelper.SendMail(model.Email,
                     "Tetas Email confirmation", $"<h1>Tetas Email confirmation</h1>" +
                                                 $"For the activation of this account " +
                                                 $"please click on the followed link:</br></br><a href = \"{tokenLink}\">Confirm Email Link</a>");

                    }
                    catch { }

                    //  ModelState.AddModelError(string.Empty, "The instructions to activate your account was send it to your Email");

                    return RedirectToAction("Index", "Home", new { message = "The instructions to activate your account was send it to your Email" });
                    //  return View(model);
                }

                ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return View(model);
        }

        [AllowAnonymous]
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

        public async Task<IActionResult> Profile(string id = "") //this id is the email
        {
            if (string.IsNullOrEmpty(id))
            {
                id = User.Identity.Name;
            }
            var user = await _userHelper.GetUserByEmailAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var profile = new ProfileModel
            {
                Name = user.Name,
                Lastname = user.Lastname,
                NickName = user.NickName,
                Phone = user.PhoneNumber,
                PictureUrl = user.PictureUrl,
                Email = user.Email,
                Bio = (user.Bio ?? string.Empty).Replace(Environment.NewLine, "<br />")
            };

            if (User.Identity.Name == id)
            {
                profile.IsMe = true;
            }
            else
            {
                var me = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                ViewBag.IsBlocked = me != null && await _moderation.IsBlockedAsync(me.Id, user.Id);
            }

            return View(profile);
        }

        public async Task<IActionResult> EditProfile()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var profile = new ProfileModel
            {
                Name = user.Name,
                Lastname = user.Lastname,
                NickName = user.NickName,
                Phone = user.PhoneNumber,
                PictureUrl = user.PictureUrl,
                Bio = user.Bio
            };

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                // var user = await userManager.FindByEmailAsync(model.Email);
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user != null)
                {
                    user.Name = model.Name;
                    user.Lastname = model.Lastname;
                    user.NickName = model.NickName;
                    user.PhoneNumber = model.Phone;
                    user.Bio = model.Bio;

                    var result = await _userHelper.UpdateUserAsync(user);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be undated.");
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "Changes Saved");
                    return RedirectToAction("Profile", new { id = user.Email });
                }
                else
                {
                    return NotFound();
                }


            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", new { id = user.Email });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userHelper.GeneratePasswordResetTokenAsync(user);
                    var link = Url.Action("ResetPassword", "Account", new
                    {
                        token,
                        email = user.Email
                    }, protocol: HttpContext.Request.Scheme);

                    try
                    {
                        _mailHelper.SendMail(user.Email,
                            "Tetas Password Reset",
                            $"<h1>Tetas Password Reset</h1>" +
                            $"To reset your password please click on the followed link:</br></br>" +
                            $"<a href = \"{link}\">Reset Password Link</a>");
                    }
                    catch { }
                }

                return RedirectToAction("Login", new { message = "If the email exists, password reset instructions were sent." });
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel
            {
                Token = token,
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", new { message = "Your password has been reset." });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(model);
                }

                return RedirectToAction("Login", new { message = "Your password has been reset." });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Block(string email)
        {
            var me = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var target = await _userHelper.GetUserByEmailAsync(email);
            if (me == null || target == null)
            {
                return NotFound();
            }

            await _moderation.BlockAsync(me.Id, target.Id);
            return RedirectToAction(nameof(Profile), new { id = email });
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(string email)
        {
            var me = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var target = await _userHelper.GetUserByEmailAsync(email);
            if (me == null || target == null)
            {
                return NotFound();
            }

            await _moderation.UnblockAsync(me.Id, target.Id);
            return RedirectToAction(nameof(Profile), new { id = email });
        }

        [AllowAnonymous]
        public IActionResult NotAuthorized()
        {
            return View();
        }

    }
}