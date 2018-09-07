using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Data;
using ToyForSI.Models;
using System;
using System.Collections.Generic;
using ToyForSI.TagHelpers;

namespace ToyForSI.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        private readonly ToyForSIContext _context;

        public DeviceController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: Device
        public async Task<IActionResult> Index(
            string SortOrder,
            string searchNo, 
            string searchSummary, 
            string searchUser, 
            string searchDepartment,
            string searchStatus,
            int? page
            )
        {
            var toyForSIContext = await _context.Device
            .AsNoTracking().Include(d => d.devModel).ThenInclude(m => m.brand)
            .Include(d => d.devModel).ThenInclude(m => m.equipmentType)
            .Include(d => d.historys).ThenInclude(i => i.toMember)
            .Include(d => d.historys).ThenInclude(i => i.toDepartment).ToListAsync();


            Dictionary<string, string> currentFilter = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(searchNo))
            {
                ViewData["SearchNo"] = searchNo;
                currentFilter.Add("searchNo", searchNo);
                toyForSIContext = toyForSIContext.Where(d => d.siSN.Contains(searchNo)).ToList();
            }

            if (!string.IsNullOrEmpty(searchSummary))
            {
                ViewData["SearchSummary"] = searchSummary;
                currentFilter.Add("searchSummary", searchSummary);
                toyForSIContext = toyForSIContext.Where(d => d.DeviceSummary.Contains(searchSummary)).ToList();
            }

            if (!string.IsNullOrEmpty(searchUser))
            {
                ViewData["SearchUser"] = searchUser;
                currentFilter.Add("searchUser", searchUser);
                toyForSIContext = toyForSIContext.Where(d => d.User.Contains(searchUser)).ToList();
            }

            if (!string.IsNullOrEmpty(searchDepartment))
            {
                ViewData["SearchDepartment"] = searchDepartment;
                currentFilter.Add("searchDepartment", searchDepartment);
                toyForSIContext = toyForSIContext.Where(d => d.UserDepartment.Contains(searchDepartment)).ToList();
            }
            int statusCode;
            if (int.TryParse(searchStatus, out statusCode))
            {
                Models.Enum.DeviceStatus dStatus = (Models.Enum.DeviceStatus)statusCode;
                ViewData["SearchStatus"] = (int)dStatus;
                currentFilter.Add("searchStatus", ((int)dStatus).ToString());
                toyForSIContext = toyForSIContext.Where(d => d.historys.LastOrDefault().deviceStatus.Equals(dStatus)).ToList();
            }

            switch (SortOrder)
            {
                case "COL1":
                    toyForSIContext = toyForSIContext.OrderBy(d => d.siSN).ToList();
                    ViewData["COL1"] = "COL1_DES";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "COL5";
                    break;
                case "COL1_DES":
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.siSN).ToList();
                    ViewData["COL1"] = "";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "COL5";
                    break;
                case "COL2":
                    toyForSIContext = toyForSIContext.OrderBy(d => d.DeviceSummary).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "COL2_DES";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "COL5";
                    break;
                case "COL2_DES":
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.DeviceSummary).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "COL5";
                    break;
                case "COL3":
                    toyForSIContext = toyForSIContext.OrderBy(d => d.User).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "COL3_DES";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "COL5";
                    break;
                case "COL3_DES":
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.User).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "COL5";
                    break;
                case "COL4":
                    toyForSIContext = toyForSIContext.OrderBy(d => d.UserDepartment).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "COL4_DES";
                    ViewData["COL5"] = "COL5";
                    break;
                case "COL4_DES":
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.UserDepartment).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "";
                    ViewData["COL5"] = "COL5";
                    break;
                case "COL5":
                    toyForSIContext = toyForSIContext.OrderBy(d => d.historys.LastOrDefault().deviceStatus).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "COL5_DES";
                    break;
                case "COL5_DES":
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.historys.LastOrDefault().deviceStatus).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "";
                    break;
                default:
                    toyForSIContext = toyForSIContext.OrderByDescending(d => d.deviceId).ToList();
                    ViewData["COL1"] = "COL1";
                    ViewData["COL2"] = "COL2";
                    ViewData["COL3"] = "COL3";
                    ViewData["COL4"] = "COL4";
                    ViewData["COL5"] = "COL5";
                    break;
            }


            //if (dStatus != Models.Enum.DeviceStatus.Unknown)
            //{
            //    ViewData["SearchStatus"] = (int)dStatus;
            //    currentFilter.Add("searchStatus", ((int)dStatus).ToString());
            //    toyForSIContext = toyForSIContext.Where(d => d.LastHistory.deviceStatus.Equals(dStatus));
            //}

            var pageOption = new MoPagerOption
            {
                CurrentPage = page ?? 1,
                PageSize = 10,
                Total = toyForSIContext.Count(),
                RouteUrl = "/Device/Index",
                CurrentSort = SortOrder,
                CurrentFilter = currentFilter
            };
            

            //分页参数
            ViewBag.PagerOption = pageOption;
            return View(toyForSIContext.Skip((pageOption.CurrentPage - 1) * pageOption.PageSize).Take(pageOption.PageSize).ToList());
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
            
            //DeviceFlowHistory history = new DeviceFlowHistory { toLocation = multidev.toLocation, flowDateTime = multidev.createTime, deviceStatus = ToyForSI.Models.Enum.DeviceStatus.Warehouse };
            //multidev.historys = new List<DeviceFlowHistory>() { history }.AsEnumerable();
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
                            historys = h.AsEnumerable()
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
