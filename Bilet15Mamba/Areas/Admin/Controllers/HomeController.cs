﻿using Microsoft.AspNetCore.Mvc;

namespace Bilet15Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
