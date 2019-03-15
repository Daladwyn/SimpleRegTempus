using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegTempus.Models;
using RegTempus.Services;
using RegTempus.ViewModels;

namespace RegTempus.Controllers
{
    public class AccountController : Controller
    {
        private IRegTempus _iRegTempus;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, IRegTempus iRegTempus)
        {
            _iRegTempus = iRegTempus;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await
                _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                var result = await
                    _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("RegisterTime", "Home");
                }
            }
            ModelState.AddModelError("", "User name/password not found");
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                { UserName = registerViewModel.UserName };
                var result =
                    await _userManager.CreateAsync
                    (user, registerViewModel.Password);
                Registrator registrator = new Registrator()
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    UserId = user.Id,
                    UserHaveStartedTimeMeasure = false,
                    StartedTimeMeasurement = 0
                };
                registrator = _iRegTempus.CreateRegistrator(registrator);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {

                }
            }
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}