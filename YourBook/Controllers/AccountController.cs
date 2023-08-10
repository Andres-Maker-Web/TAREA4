using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourBook.Models;
using YourBook.ViewModels;

namespace YourBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AccountUser> _userManager;
        private readonly SignInManager<AccountUser> _signInManager;
        public AccountController(UserManager<AccountUser> userManager, SignInManager<AccountUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ingreso invalido");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register(string? returnUrl= null)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl= null)
        {
            registerViewModel.ReturnUrl= returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AccountUser
                {
                    Email = registerViewModel.Email,
                    UserName= registerViewModel.UserName
                };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Password", "Fallo al crear usuario");
            }
            return View(registerViewModel);
        }
    }
}
