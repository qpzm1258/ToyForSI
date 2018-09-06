using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Data;
using ToyForSI.Models;

namespace ToyForSI.Controllers
{
    public class NetworkAdepterController : Controller
    {
        private readonly ToyForSIContext _context;

        public NetworkAdepterController(ToyForSIContext context)
        {
            _context = context;
        }

        // GET: NetworkAdepter
        public async Task<IActionResult> Index()
        {
            var toyForSIContext = _context.NetworkAdepter.Include(n => n.device);
            return View(await toyForSIContext.ToListAsync());
        }

        // GET: NetworkAdepter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var networkAdepter = await _context.NetworkAdepter
                .Include(n => n.device)
                .SingleOrDefaultAsync(m => m.NetworkAdepterId == id);
            if (networkAdepter == null)
            {
                return NotFound();
            }

            return View(networkAdepter);
        }

        // GET: NetworkAdepter/Create
        public IActionResult Create()
        {
            ViewData["deviceId"] = new SelectList(_context.Device, "deviceId", "contractNo");
            return View();
        }

        // POST: NetworkAdepter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NetworkAdepterId,deviceId,MACAddress")] NetworkAdepter networkAdepter)
        {
            if (ModelState.IsValid)
            {
                networkAdepter.MACAddress=networkAdepter.MACAddress.ToUpper().Replace(":","-");
                _context.Add(networkAdepter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["deviceId"] = new SelectList(_context.Device, "deviceId", "contractNo", networkAdepter.deviceId);
            return View(networkAdepter);
        }

        // GET: NetworkAdepter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var networkAdepter = await _context.NetworkAdepter.SingleOrDefaultAsync(m => m.NetworkAdepterId == id);
            if (networkAdepter == null)
            {
                return NotFound();
            }
            ViewData["deviceId"] = new SelectList(_context.Device, "deviceId", "contractNo", networkAdepter.deviceId);
            return View(networkAdepter);
        }

        // POST: NetworkAdepter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NetworkAdepterId,deviceId,MACAddress")] NetworkAdepter networkAdepter)
        {
            if (id != networkAdepter.NetworkAdepterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                networkAdepter.MACAddress=networkAdepter.MACAddress.ToUpper().Replace(":","-");
                try
                {
                    _context.Update(networkAdepter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NetworkAdepterExists(networkAdepter.NetworkAdepterId))
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
            ViewData["deviceId"] = new SelectList(_context.Device, "deviceId", "contractNo", networkAdepter.deviceId);
            return View(networkAdepter);
        }

        // GET: NetworkAdepter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var networkAdepter = await _context.NetworkAdepter
                .Include(n => n.device)
                .SingleOrDefaultAsync(m => m.NetworkAdepterId == id);
            if (networkAdepter == null)
            {
                return NotFound();
            }

            return View(networkAdepter);
        }

        // POST: NetworkAdepter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var networkAdepter = await _context.NetworkAdepter.SingleOrDefaultAsync(m => m.NetworkAdepterId == id);
            _context.NetworkAdepter.Remove(networkAdepter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NetworkAdepterExists(int id)
        {
            return _context.NetworkAdepter.Any(e => e.NetworkAdepterId == id);
        }
    }
}
