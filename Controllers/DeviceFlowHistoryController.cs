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
    public class DeviceFlowHistoryController : Controller
    {
        private readonly ToyForSIContext _context;

        public DeviceFlowHistoryController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: DeviceFlowHistory
        public async Task<IActionResult> Index()
        {
            var toyForSIContext = _context.DeviceFlowHistory.Include(d => d.device).Include(d => d.formdepartment).Include(d => d.fromMember).Include(d => d.toDepartment).Include(d => d.toMember);
            return View(await toyForSIContext.ToListAsync());
        }

        // GET: DeviceFlowHistory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceFlowHistory = await _context.DeviceFlowHistory
                .Include(d => d.device)
                .Include(d => d.formdepartment)
                .Include(d => d.fromMember)
                .Include(d => d.toDepartment)
                .Include(d => d.toMember)
                .SingleOrDefaultAsync(m => m.deviceFlowHistoryId == id);
            if (deviceFlowHistory == null)
            {
                return NotFound();
            }

            return View(deviceFlowHistory);
        }

        // GET: DeviceFlowHistory/Create
        public IActionResult Create()
        {
            ViewData["deviceId"] = new SelectList(_context.Device, "deviceId", "contractNo");
            ViewData["fromDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName");
            ViewData["fromMemberId"] = new SelectList(_context.Member, "memberId", "employeeId");
            ViewData["toDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName");
            ViewData["toMemberId"] = new SelectList(_context.Member, "memberId", "employeeId");
            return View();
        }

        // POST: DeviceFlowHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("deviceFlowHistoryId,deviceId,fromLocation,toLocation,fromDepartmentId,toDepartmentId,fromMemberId,toMemberId,flowDateTime,deviceStatus")] DeviceFlowHistory deviceFlowHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceFlowHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["deviceId"] = new SelectList(_context.Device, "deviceId", "contractNo", deviceFlowHistory.deviceId);
            ViewData["fromDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", deviceFlowHistory.fromDepartmentId);
            ViewData["fromMemberId"] = new SelectList(_context.Member, "memberId", "employeeId", deviceFlowHistory.fromMemberId);
            ViewData["toDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", deviceFlowHistory.toDepartmentId);
            ViewData["toMemberId"] = new SelectList(_context.Member, "memberId", "employeeId", deviceFlowHistory.toMemberId);
            return View(deviceFlowHistory);
        }

        // GET: DeviceFlowHistory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceFlowHistory = await _context.DeviceFlowHistory.SingleOrDefaultAsync(m => m.deviceFlowHistoryId == id);
            if (deviceFlowHistory == null)
            {
                return NotFound();
            }
            ViewData["deviceId"] = new SelectList(_context.Device, "deviceId", "contractNo", deviceFlowHistory.deviceId);
            ViewData["fromDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", deviceFlowHistory.fromDepartmentId);
            ViewData["fromMemberId"] = new SelectList(_context.Member, "memberId", "employeeId", deviceFlowHistory.fromMemberId);
            ViewData["toDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", deviceFlowHistory.toDepartmentId);
            ViewData["toMemberId"] = new SelectList(_context.Member, "memberId", "employeeId", deviceFlowHistory.toMemberId);
            return View(deviceFlowHistory);
        }

        // POST: DeviceFlowHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("deviceFlowHistoryId,deviceId,fromLocation,toLocation,fromDepartmentId,toDepartmentId,fromMemberId,toMemberId,flowDateTime,deviceStatus")] DeviceFlowHistory deviceFlowHistory)
        {
            if (id != deviceFlowHistory.deviceFlowHistoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceFlowHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceFlowHistoryExists(deviceFlowHistory.deviceFlowHistoryId))
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
            ViewData["deviceId"] = new SelectList(_context.Device, "deviceId", "contractNo", deviceFlowHistory.deviceId);
            ViewData["fromDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", deviceFlowHistory.fromDepartmentId);
            ViewData["fromMemberId"] = new SelectList(_context.Member, "memberId", "employeeId", deviceFlowHistory.fromMemberId);
            ViewData["toDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", deviceFlowHistory.toDepartmentId);
            ViewData["toMemberId"] = new SelectList(_context.Member, "memberId", "employeeId", deviceFlowHistory.toMemberId);
            return View(deviceFlowHistory);
        }

        // GET: DeviceFlowHistory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceFlowHistory = await _context.DeviceFlowHistory
                .Include(d => d.device)
                .Include(d => d.formdepartment)
                .Include(d => d.fromMember)
                .Include(d => d.toDepartment)
                .Include(d => d.toMember)
                .SingleOrDefaultAsync(m => m.deviceFlowHistoryId == id);
            if (deviceFlowHistory == null)
            {
                return NotFound();
            }

            return View(deviceFlowHistory);
        }

        // POST: DeviceFlowHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deviceFlowHistory = await _context.DeviceFlowHistory.SingleOrDefaultAsync(m => m.deviceFlowHistoryId == id);
            _context.DeviceFlowHistory.Remove(deviceFlowHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceFlowHistoryExists(int id)
        {
            return _context.DeviceFlowHistory.Any(e => e.deviceFlowHistoryId == id);
        }
    }
}
