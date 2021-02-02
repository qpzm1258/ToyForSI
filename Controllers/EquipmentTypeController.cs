using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ToyForSI.Data;
using ToyForSI.Models;

namespace ToyForSI.Controllers
{
    [Authorize]
    public class EquipmentTypeController : Controller
    {
        private readonly ToyForSIContext _context;

        public EquipmentTypeController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: EquipmentType
        public async Task<IActionResult> Index()
        {
            return View(await _context.EquipmentType.ToListAsync());
        }

        // GET: EquipmentType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentType = await _context.EquipmentType
                .SingleOrDefaultAsync(m => m.equipmentTypeId == id);
            if (equipmentType == null)
            {
                return NotFound();
            }

            return View(equipmentType);
        }

        // GET: EquipmentType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EquipmentType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("equipmentTypeId,equipmentTypeName,,life,equipmentTypeRemarks")] EquipmentType equipmentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipmentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipmentType);
        }

        // GET: EquipmentType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentType = await _context.EquipmentType.SingleOrDefaultAsync(m => m.equipmentTypeId == id);
            if (equipmentType == null)
            {
                return NotFound();
            }
            return View(equipmentType);
        }

        // POST: EquipmentType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("equipmentTypeId,equipmentTypeName,life,equipmentTypeRemarks")] EquipmentType equipmentType)
        {
            if (id != equipmentType.equipmentTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipmentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentTypeExists(equipmentType.equipmentTypeId))
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
            return View(equipmentType);
        }

        // GET: EquipmentType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentType = await _context.EquipmentType
                .SingleOrDefaultAsync(m => m.equipmentTypeId == id);
            if (equipmentType == null)
            {
                return NotFound();
            }

            return View(equipmentType);
        }

        // POST: EquipmentType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipmentType = await _context.EquipmentType.SingleOrDefaultAsync(m => m.equipmentTypeId == id);
            _context.EquipmentType.Remove(equipmentType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentTypeExists(int id)
        {
            return _context.EquipmentType.Any(e => e.equipmentTypeId == id);
        }
    }
}
