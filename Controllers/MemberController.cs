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
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ToyForSI.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly ToyForSIContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MemberController(ToyForSIContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;



        }

        // GET: Member
        public async Task<IActionResult> Index(string SortOrder, string searchName, string searchDepartment, string searchPosition, int? page)
        {
            //ViewData["NAME"] = SortOrder == "NAME" ? "NAME_DES" : "NAME";
            //ViewData["DEP"] = SortOrder == "DEP" ? "DEP_DES" : "DEP";
            //ViewData["POS"] = SortOrder == "POS" ? "POS_DES" : "POS";
            var toyForSIContext =await _context.Member.AsTracking()
            .Include(m => m.department)
            .Include(m => m.position)
            .OrderBy(d => d.department.order)
            .ToListAsync();

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
                .Include(m => m.historys)
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
        public async Task<IActionResult> Create([Bind("memberId,name,sex,employeeId,tel,mobile,departmentId,positionId,IDCard")] Member member)
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
            ViewData["srcDepId"]=member.departmentId;
            ViewData["srcPosId"]=member.positionId;
            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("memberId,name,sex,employeeId,tel,mobile,departmentId,positionId,IDCard,createTime")] Member member, string reason, int? srcDep, int? srcPos)
        {
            if (id != member.memberId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                bool writeHistory=srcDep!=member.departmentId||srcPos!=member.positionId;

                try
                {
                    if(writeHistory)
                    {
                        PersonnelTransferHistory history=new PersonnelTransferHistory();
                        history.reason=reason;
                        history.memberId=member.memberId;
                        history.flowDateTime=DateTime.Now;
                        if(srcDep.HasValue)
                        {
                            history.formDepartment= _context.Department.Where(d => d.departmentId==srcDep).SingleOrDefault().departmentName;
                        }
                        if(srcPos.HasValue)
                        {
                            history.formPosition=  _context.Position.Where(d => d.positionId==srcPos).SingleOrDefault().positionName;
                        }
                        if(member.departmentId.HasValue)
                        {
                            history.toDepartment=  _context.Department.Where(d => d.departmentId==member.departmentId).SingleOrDefault().departmentName;

                        }
                        if(member.positionId.HasValue)
                        {
                            history.toPosition=  _context.Position.Where(d => d.positionId==member.positionId).SingleOrDefault().positionName;
                        }
                        _context.Add(history);
                    }
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

        public async Task<IActionResult> ExportExcel()
        {
            var toyForSIContext =await _context.Member.AsTracking()
            .Include(m => m.department)
            .Include(m => m.position)
            .OrderBy(m => m.position.order)
            .OrderBy(d => d.department.order)
            .ToListAsync();
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "excel", "members.xlsx");
            using (var p = new ExcelPackage())
            {
                //A workbook must have at least on cell, so lets add one... 
                var ws=p.Workbook.Worksheets.Add("Members");
                ws.Cells.Style.WrapText = true;
                //To set values in the spreadsheet use the Cells indexer.
                ws.Cells["A1"].Value="某单位通讯录";
                ws.Cells["A1:I1"].Merge=true;
                ws.Cells["A1"].Style.Font.Name="宋体";
                ws.Cells["A1"].Style.Font.Bold =true;
                ws.Cells["A1"].Style.Font.Size=16;
                ws.Cells["A1:I1"].Style.HorizontalAlignment=OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["A1:I1"].Style.VerticalAlignment=OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Row(1).Height=22.25;
                ws.Cells["A2"].Value = "科室";
                ws.Cells["B2"].Value = "序号";
                ws.Cells["C2"].Value = "姓名";
                ws.Cells["D2"].Value = "职务";
                ws.Cells["E2"].Value = "办公电话";
                ws.Cells["F2"].Value = "移动电话";
                ws.Cells["G2"].Value = "传真电话";
                ws.Cells["I2"].Value = "性别";
                ws.Cells["A2:I2"].Style.Font.Name="宋体";
                ws.Cells["A2:I2"].Style.Font.Bold =true;
                ws.Cells["A2:I2"].Style.Font.Size=12;
                ws.Cells["A2:I2"].Style.HorizontalAlignment=OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["A2:I2"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells["A2:I2"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells["A2:I2"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells["A2:I2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                ws.Cells["A2:I2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                

                for (int i=0;i<toyForSIContext.Count;i++)
                {
                    if(toyForSIContext.ElementAt(i).departmentId.HasValue)
                    {
                        ws.Cells[i + 3, 1].Value = toyForSIContext.ElementAt(i).department.departmentName;
                    }
                    ws.Cells[i + 3, 3].Value = toyForSIContext.ElementAt(i).name;
                    if(toyForSIContext.ElementAt(i).positionId.HasValue)
                    {
                        ws.Cells[i + 3, 4].Value = toyForSIContext.ElementAt(i).position.positionName;
                    }
                    ws.Cells[i + 3, 5].Value = toyForSIContext.ElementAt(i).tel;
                    ws.Cells[i + 3, 6].Value = toyForSIContext.ElementAt(i).mobile;
                    ws.Cells[i + 3, 8].Value = i+1;
                    if( toyForSIContext.ElementAt(i).sex==Models.Enum.Sex.Female)
                    {
                        ws.Cells[i + 3, 9].Value = "女";
                    }
                    ws.Cells[i + 3, 1, i + 3, 9].Style.Font.Name="宋体";
                    ws.Cells[i + 3, 1, i + 3, 9].Style.Font.Size=12;
                    ws.Cells[i + 3, 1, i + 3, 9].Style.HorizontalAlignment=OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[i + 3, 1, i + 3, 9].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[i + 3, 1, i + 3, 9].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[i + 3, 1, i + 3, 9].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[i + 3, 1, i + 3, 9].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[i + 3, 1, i + 3, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }
                string key = ws.Cells[3,1].Value.ToString();
                int mergetop = 3;
                int mergetail = 3;
                int d_idx=1;
                for(; mergetail < toyForSIContext.Count+3;mergetail++)
                {
                    if(key != ws.Cells[mergetail,1].Value.ToString())
                    {
                        if(mergetail-1-mergetop>1)
                        {
                            ws.Cells[mergetop, 1, mergetail - 1, 1].Merge = true;
                            ws.Cells[mergetop, 7, mergetail - 1, 7].Merge = true;
                        }
                        mergetop = mergetail;
                        d_idx=1;
                        ws.Cells[mergetail , 2].Value=d_idx++;
                        key = ws.Cells[mergetail, 1].Value.ToString();
                    }
                    else
                    {
                        ws.Cells[mergetail , 2].Value=d_idx++;
                        continue;
                    }
                }
                ws.Column(1).Width=9.25+0.71;
                ws.Column(2).Width=4.38+0.71;
                ws.Column(3).Width=8.38+0.71;
                ws.Column(4).Width=17+0.71;
                ws.Column(5).Width=17.68+0.71;
                ws.Column(6).Width=13.38+0.71;
                ws.Column(7).Width=7.38+0.71;
                ws.Column(8).Width=4.5+0.71;
                ws.Column(9).Width=3.68+0.71;
                //ws.Cells[1, 1, toyForSIContext.Count + 1, 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                //ws.Cells[1, 1, toyForSIContext.Count + 1, 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                //ws.Cells[1, 1, toyForSIContext.Count + 1, 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                //ws.Cells[1, 1, toyForSIContext.Count + 1, 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                //ws.Cells[1, 1, toyForSIContext.Count + 1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                //Save the new workbook. We haven't specified the filename so use the Save as method.
                p.SaveAs(new FileInfo(path));
            }
            return File(await System.IO.File.ReadAllBytesAsync(path), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "phone.xlsx");
        }
    }
}
