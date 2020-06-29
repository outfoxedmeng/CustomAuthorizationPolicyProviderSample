using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PolicyProviderSample.Authorization;

namespace PolicyProviderSample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [MinimumAuthorize(10)]
        public IActionResult MinimumAge10()
        {
            return View("MinimumAge", 10);
        }
        [MinimumAuthorize(50)]
        public IActionResult MinimumAge50()
        {
            return View("MinimumAge", 50);
        }
    }
}