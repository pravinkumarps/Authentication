using IdentityExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            //Login functionality
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                // sign-in
                // creates the applcation cookie
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            //Register functionality
            var user = new IdentityUser() { UserName = userName, Email = "" };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Stage -1 : if you don't need to have the user to confirm his email, sign in him straight away
                // this will create an application cookie and redirects to home page and the user is authorized
                // to access the Authoize attribute gurarded method
                //var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                //if (signInResult.Succeeded)
                //{
                //    return RedirectToAction("Index");
                //}

                // Stage-2 : If you want the email to be confirmed then send a email confirmation token
                var emailResult = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                string link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code = emailResult }, Request.Scheme,Request.Host.ToString());
                
                //return RedirectToAction("ConfirmEmail", new { value = link });
                return View("ConfirmEmail", link);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> VerifyEmail(string userId,string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user==null)
            {
                return BadRequest();
            }
            var confResult = await _userManager.ConfirmEmailAsync(user, code);
            if (confResult.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        public IActionResult ConfirmEmail(string value) => View(value);
    }
}
