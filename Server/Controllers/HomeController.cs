using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
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

            //Authenticating a user using JWT
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"some_id"),
                new Claim("grandma_claim","cookie")
                
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.SecretKey);
            var symmetricKey = new SymmetricSecurityKey(secretBytes);
            var alogrithm = SecurityAlgorithms.HmacSha256;
            var signingCredential = new SigningCredentials(symmetricKey, alogrithm);
            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddHours(1),
                signingCredential
                );

            var jsonToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { accesstoken=jsonToken });
        }
    }
}
