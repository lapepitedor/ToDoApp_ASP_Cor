using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.Models;
using ToDoApp.ViewModel;
using ToDoApp.Misc;

namespace ToDoApp.Controllers
{
    public class AuthenticationController : Controller
    {
        private ILogger<AuthenticationController> logger;
        private IUserRepository userRepository;
        private EmailService mailService;
        private PasswordHelper passwordHelper;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUserRepository userRepository, EmailService mailService, PasswordHelper passwordHelper)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.mailService = mailService;
            this.passwordHelper = passwordHelper;
        }

        [HttpGet("/Authentication/Login/")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/Authentication/Authenticate/")]
        public async Task<IActionResult> Authenticate([FromForm] LoginViewModel login)
        {
            if (!ModelState.IsValid)
                return View("Login", login);

            var user = userRepository.FindByLogin(login.EMail, login.Password);
            if (user is null)
                return View("Login", user);

            var claims = user.ToClaims();
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Redirect("/");
        }

        [HttpGet("/Authentication/Logout/")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Authentication/Login");
        }

        [HttpGet("/Authentication/PasswordForgotten/")]
        public IActionResult PasswordForgotten()
        {
            return View();
        }

        [HttpPost("/Authentication/SendPasswordResetMail/")]
        public IActionResult SendPasswordResetMail([FromForm] PasswordForgottenViewModel pf)
        {
            if (!ModelState.IsValid)
                return View("PasswordForgotten", pf);

            var user = userRepository.FindByEmail(pf.EMail);
            if (user is not null)
                mailService.SendPasswortResetMail(user);

            return View();
        }

        [HttpGet("/Authentication/ResetPassword/{token}")]
        public IActionResult ResetPassword([FromRoute] string token)
        {
            var user = userRepository.FindByPasswordResetToken(token);
            if (user is null)
                return NotFound();

            return View(new PasswordResetViewModel() { Token = token });
        }

        [HttpPost("/Authentication/ResetPassword")]
        public IActionResult ResetPassword([FromForm] PasswordResetViewModel pr)
        {
            if (!ModelState.IsValid)
                return View("ResetPassword", pr);

            var user = userRepository.FindByPasswordResetToken(pr.Token);
            if (user is null)
                return View("ResetPassword", pr);

            user.PasswordHash = passwordHelper.ComputeSha256Hash(pr.Password);
            user.PasswordResetToken = string.Empty;
            userRepository.Update(user);
            return Redirect("/");
        }

        [HttpGet("/Authentication/Register/")]
        public IActionResult Register()
        {
            return View();
        }
    }
}
