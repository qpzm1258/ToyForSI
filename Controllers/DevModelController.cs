using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Data;
using ToyForSI.Models;

namespace ToyForSI.Controllers
{
    [Authorize]
    public class DevModelController : Controller
    {
        private readonly ToyForSIContext _context;

        public DevModelController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: DevModel
        public async Task<IActionResult> Index()
        {
            var toyForSIContext = _context.DevModel.Include(d => d.brand).Include(d => d.equipmentType).Include(d=>d.devices);
            return View(await toyForSIContext.ToListAsync());
        }

        // GET: DevModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devModel = await _context.DevModel
                .Include(d => d.brand)
                .Include(d => d.equipmentType)
                .SingleOrDefaultAsync(m => m.devModelId == id);
            if (devModel == null)
            {
                return NotFound();
            }

            return View(devModel);
        }

        // GET: DevModel/Create
        public IActionResult Create()
        {
            ViewData["brandId"] = new SelectList(_context.Brand, "brandId", "brandName");
            ViewData["equipmentTypeId"] = new SelectList(_context.EquipmentType, "equipmentTypeId", "equipmentTypeName");
            return View();
        }

        // POST: DevModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("devModelId,devModelName,brandId,equipmentTypeId")] DevModel devModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(devModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["brandId"] = new SelectList(_context.Brand, "brandId", "brandName", devModel.brandId);
            ViewData["equipmentTypeId"] = new SelectList(_context.EquipmentType, "equipmentTypeId", "equipmentTypeName", devModel.equipmentTypeId);
            return View(devModel);
        }

        // GET: DevModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devModel = await _context.DevModel.SingleOrDefaultAsync(m => m.devModelId == id);
            if (devModel == null)
            {
                return NotFound();
            }
            ViewData["brandId"] = new SelectList(_context.Brand, "brandId", "brandName", devModel.brandId);
            ViewData["equipmentTypeId"] = new SelectList(_context.EquipmentType, "equipmentTypeId", "equipmentTypeName", devModel.equipmentTypeId);
            return View(devModel);
        }

        // POST: DevModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("devModelId,devModelName,brandId,equipmentTypeId")] DevModel devModel)
        {
            if (id != devModel.devModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevModelExists(devModel.devModelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["brandId"] = new SelectList(_context.Brand, "brandId", "brandName", devModel.brandId);
            ViewData["equipmentTypeId"] = new SelectList(_context.EquipmentType, "equipmentTypeId", "equipmentTypeName", devModel.equipmentTypeId);
            return View(devModel);
        }

        // GET: DevModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devModel = await _context.DevModel
                .Include(d => d.brand)
                .Include(d => d.equipmentType)
                .SingleOrDefaultAsync(m => m.devModelId == id);
            if (devModel == null)
            {
                return NotFound();
            }

            return View(devModel);
        }

        // POST: DevModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devModel = await _context.DevModel.SingleOrDefaultAsync(m => m.devModelId == id);
            _context.DevModel.Remove(devModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevModelExists(int id)
        {
            return _context.DevModel.Any(e => e.devModelId == id);
        }
    }
}
