using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace PolicyProviderSample.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Signin(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(string userName, DateTime? birthDate, string returnUrl)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("A User Name is Required");
            }
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, userName));

            if (birthDate.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.DateOfBirth, birthDate.Value.ToShortDateString()));
            }

            var claimIdentity = new ClaimsIdentity(claims, "SomeTests");

            await HttpContext.SignInAsync(new ClaimsPrincipal(claimIdentity));

            return Redirect(returnUrl ?? "/");
        }


        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}