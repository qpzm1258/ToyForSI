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
    public class DepartmentValueController : Controller
    {
        private readonly ToyForSIContext _context;

        public DepartmentValueController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: DepartmentValue
        public async Task<IActionResult> Index()
        {
            var toyForSIContext = _context.DepartmentValue.Include(d => d.department).Include(d => d.departmentAttributes);
            return View(await toyForSIContext.ToListAsync());
        }

        // GET: DepartmentValue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentValue = await _context.DepartmentValue
                .Include(d => d.department)
                .Include(d => d.departmentAttributes)
                .SingleOrDefaultAsync(m => m.departmentValueId == id);
            if (departmentValue == null)
            {
                return NotFound();
            }

            return View(departmentValue);
        }

        // GET: DepartmentValue/Create
        public IActionResult Create()
        {
            ViewData["departmentId"] = new SelectList(_context.Department, "departmentId", "departmentName");
            ViewData["departmentAttributeId"] = new SelectList(_context.DepartmentAttributes, "departmentAttributeId", "departmentAttributeName");
            return View();
        }

        // POST: DepartmentValue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("departmentValueId,departmentId,departmentAttributeId,departmentValue")] DepartmentValue departmentValue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["departmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", departmentValue.departmentId);
            ViewData["departmentAttributeId"] = new SelectList(_context.DepartmentAttributes, "departmentAttributeId", "departmentAttributeName", departmentValue.departmentAttributeId);
            return View(departmentValue);
        }

        // GET: DepartmentValue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentValue = await _context.DepartmentValue.SingleOrDefaultAsync(m => m.departmentValueId == id);
            if (departmentValue == null)
            {
                return NotFound();
            }
            ViewData["departmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", departmentValue.departmentId);
            ViewData["departmentAttributeId"] = new SelectList(_context.DepartmentAttributes, "departmentAttributeId", "departmentAttributeName", departmentValue.departmentAttributeId);
            return View(departmentValue);
        }

        // POST: DepartmentValue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("departmentValueId,departmentId,departmentAttributeId,departmentValue")] DepartmentValue departmentValue)
        {
            if (id != departmentValue.departmentValueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentValueExists(departmentValue.departmentValueId))
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
            ViewData["departmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", departmentValue.departmentId);
            ViewData["departmentAttributeId"] = new SelectList(_context.DepartmentAttributes, "departmentAttributeId", "departmentAttributeName", departmentValue.departmentAttributeId);
            return View(departmentValue);
        }

        // GET: DepartmentValue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentValue = await _context.DepartmentValue
                .Include(d => d.department)
                .Include(d => d.departmentAttributes)
                .SingleOrDefaultAsync(m => m.departmentValueId == id);
            if (departmentValue == null)
            {
                return NotFound();
            }

            return View(departmentValue);
        }

        // POST: DepartmentValue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentValue = await _context.DepartmentValue.SingleOrDefaultAsync(m => m.departmentValueId == id);
            _context.DepartmentValue.Remove(departmentValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentValueExists(int id)
        {
            return _context.DepartmentValue.Any(e => e.departmentValueId == id);
        }
    }
}
