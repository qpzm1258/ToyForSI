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
    public class DepartmentController : Controller
    {
        private readonly ToyForSIContext _context;
        static ActionStep step=ActionStep.stepCount;

        public DepartmentController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            step=ActionStep.index;
            return View(await _context.Department.Include(m=>m.members).ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            step=ActionStep.details;
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department.Include(d => d.departmentValues)
                .SingleOrDefaultAsync(m => m.departmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            GetAttributes(id);
            return View(department);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            step=ActionStep.create;
            GetAttributes(null);
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("departmentId,departmentName")] Department department, IDictionary<int,string> dictionary)
        {
            step=ActionStep.create;
            if (ModelState.IsValid)
            {
                _context.Add(department);
                //await _context.SaveChangesAsync();
                foreach(KeyValuePair<int,string> value in dictionary)
                {
                    _context.Add(new DepartmentValue { departmentAttributeId = value.Key,department=department,departmentValue = value.Value });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            step=ActionStep.edit;
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department.SingleOrDefaultAsync(m => m.departmentId == id);
            if (department == null)
            {
                return NotFound();
            }
            GetAttributes(id);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("departmentId,departmentName")] Department department, IDictionary<int, string> dictionary)
        {
            step=ActionStep.edit;
            if (id != department.departmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    var departmentValues = _context.DepartmentValue.Where(v => v.departmentId == id);
                    foreach (KeyValuePair<int,string> value in dictionary)
                    {
                        DepartmentValue dv=departmentValues.SingleOrDefault(v => v.departmentValueId == value.Key);
                        if (dv != null)
                        {
                            dv.departmentValue = value.Value;
                            _context.Update(dv);
                         }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.departmentId))
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
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            step=ActionStep.delete;
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .SingleOrDefaultAsync(m => m.departmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Department.SingleOrDefaultAsync(m => m.departmentId == id);
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.departmentId == id);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyName(string departmentname,int departmentid)
        {
            if(step.Equals(ActionStep.edit))
            {
                if(_context.Department.FirstOrDefault(d => d.departmentId==departmentid).departmentName==departmentname)
                {
                    return Json(data:true);
                }
                if (_context.Department.Any(e => e.departmentName == departmentname))
                {
                    return Json(data: $"部门名' {departmentname} '已经被占用");
                }
            }

            if (_context.Department.Any(e => e.departmentName == departmentname))
            {
                return Json(data: $"部门名' {departmentname} '已经被占用");
            }

            return Json(data: true);
        }

        private void GetAttributes(int? departmentid)
        {
            // var departmentvalue = from a in _context.DepartmentAttributes
            //                       orderby a.departmentAttributeId
            //                       join v in _context.DepartmentValue on a.departmentAttributeId equals v.departmentAttributeId
            //                       //where view.departmentId == departmentid
            //                       select new
            //                       {
            //                           departmentId = v.departmentId,
            //                           departmentAttributeId = v.departmentAttributeId,
            //                           departmentValueId = v.departmentValueId,
            //                           departmentAttributeName = a.departmentAttributeName,
            //                           departmentValue = v.departmentValue
            //                       } into g
            //                       where g.departmentId == departmentid
            //                       select g;
            if (departmentid.HasValue)
            {
                SyncValueTable(departmentid);

                var departmentvalue = from a in _context.DepartmentAttributes
                                      orderby a.departmentAttributeId
                                      join v in _context.DepartmentValue on a.departmentAttributeId equals v.departmentAttributeId into g
                                      from r in g.DefaultIfEmpty()
                                      where r.departmentId == departmentid
                                      select new DepartmentAttributeAndValueView
                                      {
                                          aId = a.departmentAttributeId,
                                          dId = r.departmentId,
                                          aName = a.departmentAttributeName,
                                          vId = r.departmentValueId,
                                          value = r.departmentValue
                                      };
                ViewBag.DepartmentValue = departmentvalue.ToList();
            }
            else
            {
                var attribute = _context.DepartmentAttributes;
                List<DepartmentAttributeAndValueView> departmentvalue= new List<DepartmentAttributeAndValueView>();
                foreach(var attr in attribute)
                {
                    departmentvalue.Add(new DepartmentAttributeAndValueView { aId = attr.departmentAttributeId, aName = attr.departmentAttributeName });
                }
                ViewBag.DepartmentValue = departmentvalue;
            }

        }

        public int SyncValueTable(int? departmentid)
        {
            var novalue = _context.DepartmentAttributes.Where(n =>! _context.DepartmentValue.Where(m=>m.departmentId==departmentid).Select(m => m.departmentAttributeId).Contains(n.departmentAttributeId));
            foreach(var value in novalue.ToList())
            {
                DepartmentValue v = new DepartmentValue();
                v.departmentId = (int)departmentid;
                v.departmentAttributeId = value.departmentAttributeId;
                _context.Add(v);
            }
            _context.SaveChanges();
            return 0;
        }

        public int GetMemberCount(int departmentid)
        {
            return _context.Member.Count(m=>m.departmentId==departmentid);
        }
    }
}
