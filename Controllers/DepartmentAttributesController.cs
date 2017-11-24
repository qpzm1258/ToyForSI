using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Data;
using ToyForSI.Models;
using ToyForSI.Controllers.Enum;

namespace ToyForSI.Controllers
{
    public class DepartmentAttributesController : Controller
    {
        private readonly ToyForSIContext _context;
        private static ActionStep step=ActionStep.stepCount;

        public DepartmentAttributesController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: DepartmentAttributes
        public async Task<IActionResult> Index()
        {
            step=ActionStep.index;
            return View(await _context.DepartmentAttributes.ToListAsync());
        }

        // GET: DepartmentAttributes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            step=ActionStep.details;
            if (id == null)
            {
                return NotFound();
            }

            var departmentAttributes = await _context.DepartmentAttributes
                .SingleOrDefaultAsync(m => m.departmentAttributeId == id);
            if (departmentAttributes == null)
            {
                return NotFound();
            }

            return View(departmentAttributes);
        }

        // GET: DepartmentAttributes/Create
        public IActionResult Create()
        {
            step=ActionStep.create;
            return View();
        }

        // POST: DepartmentAttributes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("departmentAttributeId,departmentAttributeName,departmentAttributeType,valueLength,isRequired,valueRegEx")] DepartmentAttributes departmentAttributes)
        {
            step=ActionStep.create;
            if (ModelState.IsValid)
            {
                _context.Add(departmentAttributes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentAttributes);
        }

        // GET: DepartmentAttributes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            step=ActionStep.edit;
            if (id == null)
            {
                return NotFound();
            }

            var departmentAttributes = await _context.DepartmentAttributes.SingleOrDefaultAsync(m => m.departmentAttributeId == id);
            if (departmentAttributes == null)
            {
                return NotFound();
            }
            return View(departmentAttributes);
        }

        // POST: DepartmentAttributes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("departmentAttributeId,departmentAttributeName,departmentAttributeType,valueLength,isRequired,valueRegEx")] DepartmentAttributes departmentAttributes)
        {
            step=ActionStep.edit;
            if (id != departmentAttributes.departmentAttributeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentAttributes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentAttributesExists(departmentAttributes.departmentAttributeId))
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
            return View(departmentAttributes);
        }

        // GET: DepartmentAttributes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            step=ActionStep.delete;
            if (id == null)
            {
                return NotFound();
            }

            var departmentAttributes = await _context.DepartmentAttributes
                .SingleOrDefaultAsync(m => m.departmentAttributeId == id);
            if (departmentAttributes == null)
            {
                return NotFound();
            }

            return View(departmentAttributes);
        }

        // POST: DepartmentAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentAttributes = await _context.DepartmentAttributes.SingleOrDefaultAsync(m => m.departmentAttributeId == id);
            _context.DepartmentAttributes.Remove(departmentAttributes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentAttributesExists(int id)
        {
            return _context.DepartmentAttributes.Any(e => e.departmentAttributeId == id);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyName(string departmentattributename, int departmentAttributeid)
        {
            if(step.Equals(ActionStep.edit))
            {
                if(_context.DepartmentAttributes.FirstOrDefault(d => d.departmentAttributeId==departmentAttributeid).departmentAttributeName==departmentattributename)
                {
                    return Json(data:true);
                }
                if (_context.DepartmentAttributes.Any(e => e.departmentAttributeName == departmentattributename))
                {
                    return Json(data: $"属性名' {departmentattributename} '已经被占用");
                }
            }
            if (_context.DepartmentAttributes.Any(e => e.departmentAttributeName == departmentattributename))
            {
                return Json(data: $"属性名' {departmentattributename} '已经被占用");
            }

            return Json(data: true);
        }
    }
}
