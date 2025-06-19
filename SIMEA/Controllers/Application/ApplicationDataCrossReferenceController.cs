using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIMEA.Data;
using SIMEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class ApplicationDataCrossReferenceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationDataCrossReferenceController> _logger;

        public ApplicationDataCrossReferenceController(ApplicationDbContext context, ILogger<ApplicationDataCrossReferenceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ApplicationDataCrossReference
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ApplicationDataCrossReferenceController.Index called by {user}", User.Identity.Name);
            return View(await _context.ApplicationDataCrossReferences.ToListAsync());
        }

        // GET: ApplicationDataCrossReference/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.ApplicationDataCrossReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossRef == null)
            {
                return NotFound();
            }

            return View(crossRef);
        }

        // GET: ApplicationDataCrossReference/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationDataCrossReference/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationDataCrossReference crossReferences)
        {
            _logger.LogInformation("ApplicationDataCrossReferenceController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                crossReferences.UserCreated = User.Identity.Name;
                crossReferences.DateCreated = DateTime.UtcNow;
                crossReferences.Dependencies ??= new List<string>();
                _context.Add(crossReferences);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(crossReferences);
        }

        // GET: ApplicationDataCrossReference/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.ApplicationDataCrossReferences.FindAsync(id);
            if (crossRef == null)
            {
                return NotFound();
            }
            return View(crossRef);
        }

        // POST: ApplicationDataCrossReference/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ApplicationName,DataEntity,DataUsage,DataSensitivity,AccessControls,Owner,DataRetentionPolicy,Dependencies")] 
            ApplicationDataCrossReference crossRef)
        {
            _logger.LogInformation("ApplicationDataCrossReferenceController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != crossRef.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    crossRef.UserModified = User.Identity.Name;
                    crossRef.DateModified = DateTime.UtcNow;
                    crossRef.Dependencies ??= new List<string>();
                    _context.Update(crossRef);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrossRefExists(crossRef.Id))
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
            return View(crossRef);
        }

        // GET: ApplicationDataCrossReference/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.ApplicationDataCrossReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossRef == null)
            {
                return NotFound();
            }

            return View(crossRef);
        }

        // POST: ApplicationDataCrossReference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crossRef = await _context.ApplicationDataCrossReferences.FindAsync(id);
            _context.ApplicationDataCrossReferences.Remove(crossRef);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrossRefExists(int id)
        {
            return _context.ApplicationDataCrossReferences.Any(e => e.Id == id);
        }
    }
}