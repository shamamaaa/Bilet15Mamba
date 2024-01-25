using Bilet15Mamba.Areas.Admin.ViewModels;
using Bilet15Mamba.DAL;
using Bilet15Mamba.Models;
using Bilet15Mamba.Utilities.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public async Task<IActionResult> Index(int page = 1)
        {
            double count = await _context.Employees.CountAsync();
            List<Employee> employees = await _context.Employees.Skip((page - 1) * 5).Take(5).Include(x => x.Position).ToListAsync();

            PaginationVm<Employee> paginationVm = new PaginationVm<Employee>
            {
                TotalPage = Math.Ceiling(count / 5),
                CurrentPage = page,
                Items = employees
            };
            return View(paginationVm);
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
            bool result = _context.Positions.Any(x => x.Id == employeeVm.PositionId);
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

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            UpdateEmployeeVm employeeVm = new UpdateEmployeeVm
            {
                Name = existed.Name,
                Surname = existed.Surname,
                PositionId = existed.PositionId,
                ImageUrl = existed.ImageUrl,
                FbLink = existed.FbLink,
                TwLink = existed.TwLink,
                IgLink = existed.IgLink,
                LinLink = existed.LinLink
            };
            GetList(employeeVm);
            return View(employeeVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateEmployeeVm employeeVm)
        {
            if (!ModelState.IsValid)
            {
                GetList(employeeVm);
                return View(employeeVm);
            }
            var existed = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            bool result = _context.Positions.Any(x => x.Id == employeeVm.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Position not found");
                GetList(employeeVm);
                return View(employeeVm);
            }
            if (employeeVm.Photo is not null)
            {
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
                string newimage = await employeeVm.Photo.CreateFile(_env.WebRootPath, "assets", "img", "team");
                existed.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "img", "team");
                existed.ImageUrl = newimage;
            }
            existed.Name = employeeVm.Name;
            existed.Surname = employeeVm.Surname;
            existed.PositionId = employeeVm.PositionId;
            existed.FbLink = employeeVm.FbLink;
            existed.IgLink = employeeVm.IgLink;
            existed.LinLink = employeeVm.LinLink;
            existed.TwLink = employeeVm.TwLink;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            existed.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "img", "team");

            _context.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Employees.Include(x=>x.Position).FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            return View(existed);
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
