using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Models;
using ToyForSI.ViewModels;
using ToyForSI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ToyForSI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToyForSIContext _context;

        public HomeController(ToyForSIContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<DataPoint> departmentPersonCount = new List<DataPoint>() ;
            var departments=await _context.Department.Include(d => d.members).AsNoTracking().ToListAsync();
            if(departments.Count!=0)
            {
                foreach(var d in departments)
                {
                    departmentPersonCount.Add(new DataPoint(d.departmentName,d.memberCount));
                }
            }
            List<DataPoint> deviceTypesCount = new List<DataPoint>();
            var deviceTypes= await _context.EquipmentType.Include(d => d.devModels).ThenInclude(m=>m.devices).AsNoTracking().ToListAsync();
            if(deviceTypes.Count!=0)
            {
                foreach(var d in deviceTypes)
                {
                    int totalCount = 0;
                    if(d.devModels!=null)
                    {
                        foreach(var m in d.devModels)
                        {
                            totalCount += m.devCount;
                        }
                    }
                    deviceTypesCount.Add(new DataPoint(d.equipmentTypeName,totalCount));
                }
            }
            List<DataPoint> deviceBrandCount = new List<DataPoint>();
            var deviceBrand= await _context.Brand.Include(d => d.devModels).ThenInclude(m=>m.devices).AsNoTracking().ToListAsync();
            if(deviceBrand.Count!=0)
            {
                foreach(var d in deviceBrand)
                {
                    int totalCount = 0;
                    if(d.devModels!=null)
                    {
                        foreach(var m in d.devModels)
                        {
                            totalCount += m.devCount;
                        }
                    }
                    deviceBrandCount.Add(new DataPoint(d.brandName,totalCount));
                }
            }

            var devices = await _context.Device
                .Include(d => d.historys).ThenInclude(i => i.toMember)
                .Include(d => d.historys).ThenInclude(i => i.toDepartment)
                .AsNoTracking().ToListAsync();

            var deviceGroupByDepartment = devices.GroupBy(c => c.UserDepartment);

            var deviceGroupByStatus = devices.GroupBy(c => c.LastHistory.deviceStatus);
            List<DataPoint> deviceGroupByDepartmentCount = new List<DataPoint>();
            if (deviceGroupByDepartment != null)
            {
                foreach (var d in deviceGroupByDepartment)
                {
                    deviceGroupByDepartmentCount.Add(new DataPoint(d.Key==""?"无":d.Key, d.Count()));
                }
            }

            List<DataPoint> deviceGroupByStatusCount = new List<DataPoint>();
            if (deviceGroupByStatus != null)
            {
                foreach (var d in deviceGroupByStatus)
                {

                    string status = d.Key.GetType().GetMember(d.Key.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.Name ?? d.Key.ToString();

                    deviceGroupByStatusCount.Add(new DataPoint(status , d.Count()));
                }
            }

            ViewBag.deviceGroupByStatusCount = JsonConvert.SerializeObject(deviceGroupByStatusCount);
            ViewBag.deviceGroupByDepartmentCount = JsonConvert.SerializeObject(deviceGroupByDepartmentCount);
            ViewBag.deviceTypesCount = JsonConvert.SerializeObject(deviceTypesCount);
            ViewBag.departmentPersonCount = JsonConvert.SerializeObject(departmentPersonCount);
            ViewBag.deviceBrandCount = JsonConvert.SerializeObject(deviceBrandCount);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
