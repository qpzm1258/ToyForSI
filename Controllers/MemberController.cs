using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Data;
using ToyForSI.Models;
using ToyForSI.TagHelpers;
using System.Collections.Generic;

namespace ToyForSI.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly ToyForSIContext _context;

        public MemberController(ToyForSIContext context)
        {
            _context = context;

            
        }

        // GET: Member
        public async Task<IActionResult> Index(string SortOrder, string searchName, string searchDepartment, string searchPosition, int? page)
        {
            //ViewData["NAME"] = SortOrder == "NAME" ? "NAME_DES" : "NAME";
            //ViewData["DEP"] = SortOrder == "DEP" ? "DEP_DES" : "DEP";
            //ViewData["POS"] = SortOrder == "POS" ? "POS_DES" : "POS";
            var toyForSIContext =await _context.Member.AsTracking()
            .Include(m => m.department).Include(m => m.position).OrderBy(d => d.memberId).ToListAsync();

            Dictionary<string, string> currentFilter=new Dictionary<string,string>();

            if (!string.IsNullOrEmpty(searchName))
            {
                ViewData["SearchName"] = searchName;
                currentFilter.Add("searchName", searchName);
                toyForSIContext = toyForSIContext.Where(d => d.name.Contains(searchName)).ToList();
            }

            if (!string.IsNullOrEmpty(searchDepartment))
            {
                ViewData["SearchDepartment"] = searchDepartment;
                currentFilter.Add("searchDepartment", searchDepartment);
                toyForSIContext = toyForSIContext.Where(d => d.department.departmentName.Contains(searchDepartment)).ToList();
            }

            if (!string.IsNullOrEmpty(searchPosition))
            {
                ViewData["SearchPosition"] = searchPosition;
                currentFilter.Add("searchPosition", searchPosition);
                toyForSIContext = toyForSIContext.Where(d => d.position.positionName.Contains(searchPosition)).ToList();
            }

            switch (SortOrder)
            {
                case "NAME":
                    toyForSIContext = toyForSIContext.OrderBy(d => d.name).ToList();
                    ViewData["NAME"] = "NAME_DES";
                    ViewData["DEP"] = "DEP";
                    ViewData["POS"] = "POS";
                    break;
                case "NAME_DES":
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.name).ToList();
                    ViewData["NAME"] = "";
                    ViewData["DEP"] = "DEP";
                    ViewData["POS"] = "POS";
                    break;
                case "DEP":
                    toyForSIContext = toyForSIContext.OrderBy(d => d.departmentId).ToList();
                    ViewData["NAME"] = "NAME";
                    ViewData["DEP"] = "DEP_DES";
                    ViewData["POS"] = "POS";
                    break;
                case "DEP_DES":
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.departmentId).ToList();
                    ViewData["NAME"] = "NAME";
                    ViewData["DEP"] = "";
                    ViewData["POS"] = "POS";
                    break;
                case "POS":
                    toyForSIContext = toyForSIContext.OrderBy(d => d.positionId).ToList();
                    ViewData["NAME"] = "NAME";
                    ViewData["DEP"] = "DEP";
                    ViewData["POS"] = "POS_DES";
                    break;
                case "POS_DES":
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.positionId).ToList();
                    ViewData["NAME"] = "NAME";
                    ViewData["DEP"] = "DEP";
                    ViewData["POS"] = "";
                    break;
                default:
                    toyForSIContext = toyForSIContext.OrderBy(d => d.memberId).ToList();
                    ViewData["NAME"] = "NAME";
                    ViewData["DEP"] = "DEP";
                    ViewData["POS"] = "POS";
                    break;
            }

            var pageOption = new MoPagerOption
            {
                CurrentPage = page??1,
                PageSize = 10,
                Total = toyForSIContext.Count(),
                RouteUrl = "/Member/Index",
                CurrentSort=SortOrder,
                CurrentFilter=currentFilter
            };

            //分页参数
            ViewBag.PagerOption = pageOption;
            return View(toyForSIContext.Skip((pageOption.CurrentPage - 1) * pageOption.PageSize).Take(pageOption.PageSize).ToList());
            //return View(await toyForSIContext.ToListAsync());
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.department)
                .Include(m => m.position)
                .SingleOrDefaultAsync(m => m.memberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            ViewData["departmentId"] = new SelectList(_context.Department, "departmentId", "departmentName");
            ViewData["positionId"] = new SelectList(_context.Position, "positionId", "positionName");
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("memberId,name,sex,employeeId,departmentId,positionId,IDCard")] Member member)
        {
            member.createTime=DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["departmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", member.departmentId);
            ViewData["positionId"] = new SelectList(_context.Position, "positionId", "positionName", member.positionId);
            return View(member);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.SingleOrDefaultAsync(m => m.memberId == id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["departmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", member.departmentId);
            ViewData["positionId"] = new SelectList(_context.Position, "positionId", "positionName", member.positionId);
            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("memberId,name,sex,employeeId,departmentId,positionId,IDCard,createTime")] Member member)
        {
            if (id != member.memberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.memberId))
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
            ViewData["departmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", member.departmentId);
            ViewData["positionId"] = new SelectList(_context.Position, "positionId", "positionName", member.positionId);
            return View(member);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.department)
                .Include(m => m.position)
                .SingleOrDefaultAsync(m => m.memberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.SingleOrDefaultAsync(m => m.memberId == id);
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.memberId == id);
        }
    }
}
