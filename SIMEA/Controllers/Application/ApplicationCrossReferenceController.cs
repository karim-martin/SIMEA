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
    public class ApplicationCrossReferenceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationCrossReferenceController> _logger;

        public ApplicationCrossReferenceController(ApplicationDbContext context, ILogger<ApplicationCrossReferenceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ApplicationToApplicationCrossReference
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ApplicationCrossReferenceController.Index called by {user}", User.Identity.Name);
            return View(await _context.ApplicationToApplicationCrossReferences.ToListAsync());
        }

        // GET: ApplicationToApplicationCrossReference/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.ApplicationToApplicationCrossReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossRef == null)
            {
                return NotFound();
            }

            return View(crossRef);
        }

        // GET: ApplicationToApplicationCrossReference/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationToApplicationCrossReference/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationToApplicationCrossReference crossReferences)
        {
            _logger.LogInformation("ApplicationCrossReferenceController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                crossReferences.UserCreated = User.Identity.Name;
                crossReferences.DateCreated = DateTime.UtcNow;
                
                // Initialize Dependencies collection if null
                crossReferences.Dependencies ??= new List<string>();
                
                _context.Add(crossReferences);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(crossReferences);
        }

        // GET: ApplicationToApplicationCrossReference/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.ApplicationToApplicationCrossReferences.FindAsync(id);
            if (crossRef == null)
            {
                return NotFound();
            }
            return View(crossRef);
        }

        // POST: ApplicationToApplicationCrossReference/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,SourceApplication,TargetApplication,IntegrationType,DataExchanged,Frequency,Owner,SecurityRequirements,Dependencies")] 
            ApplicationToApplicationCrossReference crossRef)
        {
            _logger.LogInformation("ApplicationCrossReferenceController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
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
                    
                    // Initialize Dependencies collection if null
                    crossRef.Dependencies ??= new List<string>();
                    
                    _context.Update(crossRef);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrossReferenceExists(crossRef.Id))
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

        // GET: ApplicationToApplicationCrossReference/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.ApplicationToApplicationCrossReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossRef == null)
            {
                return NotFound();
            }

            return View(crossRef);
        }

        // POST: ApplicationToApplicationCrossReference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crossRef = await _context.ApplicationToApplicationCrossReferences.FindAsync(id);
            _context.ApplicationToApplicationCrossReferences.Remove(crossRef);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrossReferenceExists(int id)
        {
            return _context.ApplicationToApplicationCrossReferences.Any(e => e.Id == id);
        }
    }
}