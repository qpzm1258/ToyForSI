using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Data;
using ToyForSI.Models;
using ToyForSI.Controllers.Enum;

namespace ToyForSI.Controllers
{
    [Authorize]
    public class PositionController : Controller
    {
        private readonly ToyForSIContext _context;
        static ActionStep step=ActionStep.stepCount;

        public PositionController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: Position
        public async Task<IActionResult> Index()
        {
            return View(await _context.Position.OrderBy(p => p.order).ToListAsync());
        }

        // GET: Position/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Position
                .SingleOrDefaultAsync(m => m.positionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: Position/Create
        public IActionResult Create()
        {
            step=ActionStep.create;
            return View();
        }

        // POST: Position/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("positionId,positionName,order,positionAbstract")] Position position)
        {
            step=ActionStep.create;
            if (ModelState.IsValid)
            {
                _context.Add(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        // GET: Position/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            step=ActionStep.edit;
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Position.SingleOrDefaultAsync(m => m.positionId == id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        // POST: Position/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("positionId,positionName,order,positionAbstract")] Position position)
        {
            step=ActionStep.edit;
            if (id != position.positionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(position);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(position.positionId))
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
            return View(position);
        }

        // GET: Position/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Position
                .SingleOrDefaultAsync(m => m.positionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: Position/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var position = await _context.Position.SingleOrDefaultAsync(m => m.positionId == id);
            _context.Position.Remove(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            return _context.Position.Any(e => e.positionId == id);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyName(string positionName,int positionId)
        {
            if(step.Equals(ActionStep.edit))
            {
                if(_context.Position.FirstOrDefault(d => d.positionId==positionId).positionName==positionName)
                {
                    return Json(data:true);
                }
                if (_context.Position.Any(e => e.positionName == positionName))
                {
                    return Json(data: $"职位名' {positionName} '已经被占用");
                }
            }

            if (_context.Position.Any(e => e.positionName == positionName))
            {
                return Json(data: $"职位名' {positionName} '已经被占用");
            }

            return Json(data: true);
        }

    }
}
