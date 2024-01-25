using Bilet15Mamba.Areas.Admin.ViewModels;
using Bilet15Mamba.DAL;
using Bilet15Mamba.Models;
using Bilet15Mamba.Utilities.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Bilet15Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            double count = await _context.Employees.CountAsync();
            List<Employee> employees = await _context.Employees.Skip((page-1)*5).Take(5).Include(x=>x.Position).ToListAsync();

            PaginationVm<Employee> paginationVm = new PaginationVm<Employee>
            {
                TotalPage = Math.Ceiling(count / 5),
                CurrentPage = page,
                Items = employees
            };
            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            CreateEmployeeVm employeeVm = new CreateEmployeeVm();
            GetList(employeeVm);
            return View(employeeVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVm employeeVm)
        {
            if (!ModelState.IsValid)
            {
                GetList(employeeVm);
                return View(employeeVm);
            }
            bool result = _context.Positions.Any(x=>x.Id==employeeVm.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Position not found");
                GetList(employeeVm);
                return View(employeeVm);
            }
            if (!employeeVm.Photo.CheckType())
            {
                ModelState.AddModelError("Photo", "Photo type is not valid.");
                GetList(employeeVm);
                return View(employeeVm);
            }
            if (!employeeVm.Photo.CheckSize())
            {
                ModelState.AddModelError("Photo", "Photo size is not valid.");
                GetList(employeeVm);
                return View(employeeVm);
            }
            string filename = await employeeVm.Photo.CreateFile(_env.WebRootPath, "assets", "img", "team");
            Employee employee = new Employee
            {
                Name = employeeVm.Name,
                Surname = employeeVm.Surname,
                PositionId = employeeVm.PositionId,
                ImageUrl = filename,
                FbLink = employeeVm.FbLink,
                TwLink = employeeVm.TwLink,
                IgLink = employeeVm.IgLink,
                LinLink = employeeVm.LinLink
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private void GetList(CreateEmployeeVm employeeVm)
        {
            employeeVm.Positions = new(_context.Positions, "Id", "Name");
        }
        private void GetList(UpdateEmployeeVm employeeVm)
        {
            employeeVm.Positions = new(_context.Positions, "Id", "Name");
        }
    }
}
