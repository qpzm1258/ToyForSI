using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Data;
using ToyForSI.Models;
using System;
using System.Collections.Generic;
using ToyForSI.TagHelpers;

namespace ToyForSI.Controllers
{
    public class DeviceController : Controller
    {
        private readonly ToyForSIContext _context;

        public DeviceController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: Device
        public async Task<IActionResult> Index(int? page)
        {
            var toyForSIContext = _context.Device
            .Include(d => d.devModel).ThenInclude(m=>m.brand)
            .Include(d => d.devModel).ThenInclude(m=>m.equipmentType)
            .Include(d => d.historys)
            .AsNoTracking();
            var pageOption = new MoPagerOption
            {
                CurrentPage = page??1,
                PageSize = 20,
                Total = await toyForSIContext.CountAsync(),
                RouteUrl = "/Device/Index"
            };

            //分页参数
            ViewBag.PagerOption = pageOption;
            return View(await toyForSIContext.OrderByDescending(d=>d.deviceId).Skip((pageOption.CurrentPage - 1) * pageOption.PageSize).Take(pageOption.PageSize).ToListAsync());
        }

        // GET: Device/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Device
                .Include(d => d.devModel).ThenInclude(m=>m.brand)
                .Include(d => d.devModel).ThenInclude(m=>m.equipmentType)
                .Include(d=>d.historys).ThenInclude(c => c.formdepartment)
                .Include(d=>d.historys).ThenInclude(c => c.toDepartment)
                .Include(d=>d.historys).ThenInclude(c => c.fromMember)
                .Include(d=>d.historys).ThenInclude(c => c.toMember)
                .SingleOrDefaultAsync(m => m.deviceId == id);
                //device.historys.OrderByDescending(h=>h.deviceFlowHistoryId);
                device.historys=device.historys.OrderByDescending(h=>h.deviceFlowHistoryId).ToList();
            if (device == null)
            {
                return NotFound();
            }
            ViewData["SiSN"] = device.siSN;
            return View(device);
        }

        // GET: Device/Create
        public IActionResult Create()
        {
            ViewData["devModelId"] = new SelectList(_context.DevModel, "devModelId", "devModelName");
            ViewData["createTime"] = DateTime.Now;
            return View();
        }

        // POST: Device/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("deviceId,contractNo,devModelId,createTime,inWareHouse,devCount,toLocation")] MultiDev multidev)
        {
            if (ModelState.IsValid)
            {
                //List<Device> devices=new List<Device>();
                for (int idx = 0; idx != multidev.devCount; idx++)
                {
                    DevModel devModel = _context.DevModel.FirstOrDefault(d => d.devModelId == multidev.devModelId);
                    DeviceFlowHistory history = new DeviceFlowHistory { toLocation = multidev.toLocation,  flowDateTime = multidev.createTime, deviceStatus=ToyForSI.Models.Enum.DeviceStatus.Warehouse };
                    List<DeviceFlowHistory> h = new List<DeviceFlowHistory>
                    {
                        history
                    };
                    Device device = 
                        new Device { contractNo = multidev.contractNo,
                            devModelId = multidev.devModelId,
                            createTime = multidev.createTime,
                            inWareHouse = multidev.inWareHouse,
                            historys=h
                        };
                    _context.Add(device);
                }
                await _context.SaveChangesAsync();
                //foreach (Device d in devices)
                //{
                //    DeviceFlowHistory history = new DeviceFlowHistory { toLocation = multidev.toLocation, deviceId = d.deviceId, transferDateTime = DateTime.Now };
                //    _context.Add(history);
                //}
                //await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["devModelId"] = new SelectList(_context.DevModel, "devModelId", "devModelName", multidev.devModelId);
            return View(multidev);
        }

        // GET: Device/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Device.SingleOrDefaultAsync(m => m.deviceId == id);
            if (device == null)
            {
                return NotFound();
            }
            ViewData["devModelId"] = new SelectList(_context.DevModel, "devModelId", "devModelName", device.devModelId);
            return View(device);
        }

        // POST: Device/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("deviceId,contractNo,devModelId,createTime,inWareHouse")] Device device)
        {
            if (id != device.deviceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.deviceId))
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
            ViewData["devModelId"] = new SelectList(_context.DevModel, "devModelId", "devModelName", device.devModelId);
            return View(device);
        }

        // GET: Device/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Device
                .Include(d => d.devModel)
                .SingleOrDefaultAsync(m => m.deviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Device.SingleOrDefaultAsync(m => m.deviceId == id);
            _context.Device.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _context.Device.Any(e => e.deviceId == id);
        }
    }
}
