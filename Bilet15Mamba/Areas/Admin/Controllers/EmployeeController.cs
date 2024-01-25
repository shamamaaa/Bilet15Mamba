using Microsoft.AspNetCore.Mvc;

namespace Bilet15Mamba.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
