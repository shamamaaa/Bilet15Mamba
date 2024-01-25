using Bilet15Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bilet15Mamba.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}