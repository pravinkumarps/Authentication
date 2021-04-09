using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Controllers.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Prav"),
                new Claim(ClaimTypes.Email,"abc@abc.com")
            };

            var anotherClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Subrahmanya"),
                new Claim("CustomClaim","SuperMan")
            };

            var userIdentity = new ClaimsIdentity(userClaims, "CustomIdentity");
            var anotherIdentity = new ClaimsIdentity(anotherClaims, "OtherIdentity");

            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity, anotherIdentity });

            HttpContext.SignInAsync(userPrincipal);
            return RedirectToAction("Index");
        }
    }
}
