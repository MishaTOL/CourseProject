using System.Threading.Tasks;
using ITNews.Core.Domain;
using ITNews.Services.Photos;
using ITNews.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ITNews.Services.Email;
using Microsoft.AspNetCore.Authorization;

namespace ITNews.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IPhotoService photoService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IPhotoService photoService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.photoService = photoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if(signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if(ModelState.IsValid)
            {
                var member = await userManager.FindByNameAsync(model.UserName) ??
                    await userManager.FindByEmailAsync(model.UserName);



                if(member == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    model.ReturnUrl = returnUrl;
                    return View(model);
                }

                //if (member != null)
                //{
                //    if (!await userManager.IsEmailConfirmedAsync(member))
                //    {
                //        ModelState.AddModelError(string.Empty, "Please, confirm your email.");
                //    }
                //}

                var result = await signInManager.PasswordSignInAsync(member, model.Password,
                    isPersistent: model.RememberMe, lockoutOnFailure: false);

                if(result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.EmailAddress,
                };

                if (model.File != null)
                {
                    var photoLocation = await photoService.UploadAsync(model.File);
                    user.ProfilePictureUrl = photoLocation;
                }

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.EmailAddress, "Confirm your account",
                        $"Confirm your registration by clicking on the link: <a href='{callbackUrl}'>link</a>");

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "reader");
                await userManager.AddToRoleAsync(user, "writer");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Error");
            }
        }
    }
}