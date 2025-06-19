using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class CapabilityMapController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CapabilityMapController> _logger;

        public CapabilityMapController(ApplicationDbContext context, ILogger<CapabilityMapController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: CapabilityMap
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("CapabilityMapController.Index called by {user}", User.Identity.Name);
            return View(await _context.CapabilityMaps.ToListAsync());
        }

        // GET: CapabilityMap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("CapabilityMapController.Details called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var capabilityMap = await _context.CapabilityMaps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (capabilityMap == null)
            {
                return NotFound();
            }

            return View(capabilityMap);
        }

        // GET: CapabilityMap/Create
        public IActionResult Create()
        {
            _logger.LogInformation("CapabilityMapController.Create GET called by {user}", User.Identity.Name);
            return View();
        }

        // POST: CapabilityMap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CapabilityMap capabilityMaps)
        {
            _logger.LogInformation("CapabilityMapController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                capabilityMaps.UserCreated = User.Identity.Name;
                capabilityMaps.DateCreated = DateTime.UtcNow;
                
                // Initialize collections if null
                capabilityMaps.Dependencies ??= new List<string>();
                capabilityMaps.KeyProcesses ??= new List<string>();
                capabilityMaps.Gaps ??= new List<string>();
                
                _context.Add(capabilityMaps);
                await _context.SaveChangesAsync();
                _logger.LogInformation("CapabilityMap created with ID {id} by {user}", capabilityMaps.Id, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(capabilityMaps);
        }

        // GET: CapabilityMap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("CapabilityMapController.Edit GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var capabilityMap = await _context.CapabilityMaps.FindAsync(id);
            if (capabilityMap == null)
            {
                return NotFound();
            }
            return View(capabilityMap);
        }

        // POST: CapabilityMap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,CapabilityName,CapabilityDescription,MaturityLevel,Owner,Dependencies,KeyProcesses,Gaps")] 
            CapabilityMap capabilityMap)
        {
            _logger.LogInformation("CapabilityMapController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != capabilityMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    capabilityMap.UserModified = User.Identity.Name;
                    capabilityMap.DateModified = DateTime.UtcNow;
                    
                    // Initialize collections if null
                    capabilityMap.Dependencies ??= new List<string>();
                    capabilityMap.KeyProcesses ??= new List<string>();
                    capabilityMap.Gaps ??= new List<string>();
                    
                    _context.Update(capabilityMap);
                    await _context.SaveChangesAsync();
                     _logger.LogInformation("CapabilityMap updated with ID {id} by {user}", capabilityMap.Id, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapabilityMapExists(capabilityMap.Id))
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
            return View(capabilityMap);
        }

        // GET: CapabilityMap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("CapabilityMapController.Delete GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var capabilityMap = await _context.CapabilityMaps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (capabilityMap == null)
            {
                return NotFound();
            }

            return View(capabilityMap);
        }

        // POST: CapabilityMap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("CapabilityMapController.DeleteConfirmed POST called for id {id} by {user}", id, User.Identity.Name);
            var capabilityMap = await _context.CapabilityMaps.FindAsync(id);
            _context.CapabilityMaps.Remove(capabilityMap);
            await _context.SaveChangesAsync();
            _logger.LogInformation("CapabilityMap deleted with ID {id} by {user}", id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool CapabilityMapExists(int id)
        {
            return _context.CapabilityMaps.Any(e => e.Id == id);
        }
    }
}