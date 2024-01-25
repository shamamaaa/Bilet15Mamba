using Bilet15Mamba.Areas.Admin.ViewModels;
using Bilet15Mamba.DAL;
using Bilet15Mamba.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bilet15Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            double count = await _context.Settings.CountAsync();
            List<Setting> settings = await _context.Settings.Skip((page - 1) * 5).Take(5).ToListAsync();

            PaginationVm<Setting> paginationVm = new PaginationVm<Setting>
            {
                TotalPage = Math.Ceiling(count / 5),
                CurrentPage = page,
                Items = settings
            };
            return View(paginationVm);
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            UpdateSettingVm settingVm = new UpdateSettingVm
            {
                Key=existed.Key, 
                Value=existed.Value
            };
            return View(settingVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingVm settingVm)
        {
            if (!ModelState.IsValid)
            {
                return View(settingVm);
            }
            var existed = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            bool result = _context.Settings.Any(x => x.Key.Trim().ToLower() == settingVm.Key.Trim().ToLower() && x.Id!=id);
            if (result)
            {
                ModelState.AddModelError("Key","This key is already exists.");
                return View(settingVm);
            }
            existed.Value = settingVm.Value;
            existed.Key=settingVm.Key;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
