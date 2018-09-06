using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Data;
using ToyForSI.ViewModels;
using ToyForSI.Models;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToyForSI.Controllers
{
    [Authorize]
    public class DeviceFlowController : Controller
    {
        private readonly ToyForSIContext _context;
        public DeviceFlowController(ToyForSIContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // POST: DeviceFlow/GetDevice
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDevice(SiSN model)
        {
            if (ModelState.IsValid)
            {
                int devid = Int32.Parse(model.siSN.Substring(6));
                //Device device = await _context.Device.FirstOrDefaultAsync(d => d.deviceId == devid);
                return RedirectToAction(nameof(Do), new { id = devid });
            }

            return View(model);
        }

        public async Task<IActionResult> Do(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Device.Include(d=>d.historys)
                .Include(d=>d.devModel).ThenInclude(m=>m.brand)
                .Include(d=>d.devModel).ThenInclude(m=>m.equipmentType)
                .SingleOrDefaultAsync(m => m.deviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            DeviceFlowHistory history = new DeviceFlowHistory();
            history.deviceStatus = ToyForSI.Models.Enum.DeviceStatus.Normal;
            history.deviceId = device.deviceId;
            history.flowDateTime = DateTime.Now;
            history.device = device;
            ViewData["fromDepartmentId"] = new List<SelectListItem>(){new SelectListItem(){Value="",Text="无"}};
            ViewData["fromMemberId"] = new List<SelectListItem>(){new SelectListItem(){Value="",Text="无"}};
            ViewData["fromLocation"]= new List<SelectListItem>(){new SelectListItem(){Value="",Text="无"}};
            if (device.historys.ToList().Count!=0)
            {
                var lastHistory=device.historys.OrderBy(c=>c.deviceFlowHistoryId).ToList().Last();
                history.fromLocation = lastHistory.toLocation;
                history.fromDepartmentId = lastHistory.toDepartmentId;
                history.fromMemberId = lastHistory.toMemberId;
                var toDepart = lastHistory.toDepartmentId;
                if (toDepart.HasValue)
                {
                    ViewData["fromDepartmentId"] = new SelectList(_context.Department.Where(d=>d.departmentId==history.fromDepartmentId), "departmentId", "departmentName", history.fromDepartmentId);
                }
                if (lastHistory.toMemberId.HasValue)
                {
                    ViewData["fromMemberId"] = new SelectList(_context.Member, "memberId", "name",history.fromMemberId);
                }
                if (history.fromLocation!=string.Empty)
                {
                    if(history.fromLocation!=null){
                        ViewData["fromLocation"]= new List<SelectListItem>(){new SelectListItem(){Value=history.fromLocation,Text=history.fromLocation}};
                    }
                }
            }

            ViewData["deviceId"]=new List<SelectListItem>()
            {
                new SelectListItem(){Value=id.ToString(),Text=device.devModel.brand.brandName+device.devModel.devModelName+device.devModel.equipmentType.equipmentTypeName}
            };
            ViewData["toDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", history.toDepartmentId);
            ViewData["toMemberId"] = new SelectList(_context.Member, "memberId", "name", history.toMemberId);
            //ViewData["deviceStatus"]=GetEnumSelectList<ToyForSI.Models.Enum.DeviceStatus>();
           
            return View(history);
        }

        // POST: DevModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Do([Bind("deviceId,fromDepartmentId,fromMemberId,fromLocation,toDepartmentId,toMemberId,toLocation,deviceStatus,flowDateTime")] DeviceFlowHistory history, bool configNetwork)
        {
            if (ModelState.IsValid)
            {
                //List<Device> devices=new List<Device>();
                var lastHistory= await _context.DeviceFlowHistory.Where(h=>h.deviceId==history.deviceId).OrderBy(d=>d.deviceId).LastOrDefaultAsync();
                if(lastHistory!=null)
                {
                    if(lastHistory.toLocation!=history.toLocation ||
                    lastHistory.toDepartmentId!=history.toDepartmentId ||
                    lastHistory.toMemberId!=history.toMemberId ||
                    lastHistory.deviceStatus!=history.deviceStatus)    
                    {
                        _context.Add(history);
                    }
                }else
                {
                    _context.Add(history);
                }
                await _context.SaveChangesAsync();
                //foreach (Device d in devices)
                //{
                //    DeviceFlowHistory history = new DeviceFlowHistory { toLocation = multidev.toLocation, deviceId = d.deviceId, transferDateTime = DateTime.Now };
                //    _context.Add(history);
                //}
                //await _context.SaveChangesAsync();
                if(!configNetwork)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            var device = await _context.Device.Include(d=>d.historys).Include(d=>d.devModel).SingleOrDefaultAsync(m => m.deviceId == history.deviceId);
            if (device == null)
            {
                return NotFound();
            }

            history.flowDateTime = DateTime.Now;
            history.device = device;
            ViewData["fromDepartmentId"] = new List<SelectListItem>(){new SelectListItem(){Value="",Text="无"}};
            ViewData["fromMemberId"] = new List<SelectListItem>(){new SelectListItem(){Value="",Text="无"}};
            ViewData["fromLocation"]= new List<SelectListItem>(){new SelectListItem(){Value="",Text="无"}};

            ViewData["deviceId"]=new List<SelectListItem>(){new SelectListItem(){Value=history.deviceId.ToString(),Text=device.devModel.devModelName}};
            ViewData["toDepartmentId"] = new SelectList(_context.Department, "departmentId", "departmentName", history.toDepartmentId);
            ViewData["toMemberId"] = new SelectList(_context.Member, "memberId", "name", history.toMemberId);

            if (history.fromDepartmentId.HasValue)
            {
                ViewData["fromDepartmentId"] = new SelectList(_context.Department.Where(d=>d.departmentId==history.fromDepartmentId), "departmentId", "departmentName", history.fromDepartmentId);
            }
            if (history.fromMemberId.HasValue)
            {
                ViewData["fromMemberId"] = new SelectList(_context.Member, "memberId", "name",history.fromMemberId);
            }
            if (history.fromLocation!=string.Empty)
            {
                ViewData["fromLocation"]= new List<SelectListItem>(){new SelectListItem(){Value=history.fromLocation,Text=history.fromLocation}};
            }
            return View(history);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckSN(string sisn)
        {
            var devices = await _context.Device
                .Include(d=>d.devModel)
                .Where(d => d.deviceId == Int32.Parse(sisn.Substring(6))).SingleAsync();
            if (devices == null || devices.siSN != sisn)
            {
                return Json(data: false);
            }
            else
            {
                return Json(data: true);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> getMember(int? departmentId)
        {
            
            var members = _context.Member;
            if(departmentId.HasValue){
                return Json(await members.Where(d => d.departmentId==departmentId).AsNoTracking().ToListAsync());
            }
            else{
                return Json(await members.ToListAsync());
            }
        }

    }
}
