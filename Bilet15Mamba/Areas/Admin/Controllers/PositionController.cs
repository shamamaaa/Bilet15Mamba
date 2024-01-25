using Bilet15Mamba.Areas.Admin.ViewModels;
using Bilet15Mamba.DAL;
using Bilet15Mamba.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;

namespace Bilet15Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            double count = await _context.Positions.CountAsync();
            List<Position> positions = await _context.Positions.Skip((page - 1) * 5).Take(5).Include(x => x.Employees).ToListAsync();

            PaginationVm<Position> paginationVm = new PaginationVm<Position>
            {
                TotalPage = Math.Ceiling(count / 5),
                CurrentPage = page,
                Items = positions
            };
            return View(paginationVm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionVm positionVm)
        {
            if (!ModelState.IsValid)
            {
                return View(positionVm);
            }
            bool result = _context.Positions.Any(x => x.Name.Trim().ToLower() == positionVm.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "This position is already exists.");
                return View(positionVm);
            }
            Position position = new Position
            {
                Name = positionVm.Name
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            UpdatePositionVm positionVm = new UpdatePositionVm
            {
                Name = existed.Name
            };
            return View(positionVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdatePositionVm positionVm)
        {
            if (!ModelState.IsValid)
            {
                return View(positionVm);
            }
            var existed = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            bool result = _context.Positions.Any(x => x.Name.Trim().ToLower() == positionVm.Name.Trim().ToLower() && x.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "This position is already exists.");
                return View(positionVm);
            }

            existed.Name = positionVm.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            _context.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Positions.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            return View(existed);
        }
    }
}
