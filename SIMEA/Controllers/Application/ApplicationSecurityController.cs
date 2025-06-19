using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIMEA.Data;
using SIMEA.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class ApplicationSecurityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationSecurityController> _logger;

        public ApplicationSecurityController(ApplicationDbContext context, ILogger<ApplicationSecurityController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ApplicationSecurityModel
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ApplicationSecurityController.Index called by {user}", User.Identity.Name);
            return View(await _context.ApplicationSecurityModels.ToListAsync());
        }

        // GET: ApplicationSecurityModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityModel = await _context.ApplicationSecurityModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityModel == null)
            {
                return NotFound();
            }

            return View(securityModel);
        }

        // GET: ApplicationSecurityModel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationSecurityModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationSecurityModel securityModels)
        {
            _logger.LogInformation("ApplicationSecurityController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                securityModels.UserCreated = User.Identity.Name;
                securityModels.DateCreated = DateTime.UtcNow;
                
                // Initialize collections if null
                securityModels.ComplianceStandards ??= new List<string>();
                securityModels.Vulnerabilities ??= new List<string>();
                securityModels.MitigationStrategies ??= new List<string>();
                securityModels.Dependencies ??= new List<string>();
                
                _context.Add(securityModels);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(securityModels);
        }

        // GET: ApplicationSecurityModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityModel = await _context.ApplicationSecurityModels.FindAsync(id);
            if (securityModel == null)
            {
                return NotFound();
            }
            return View(securityModel);
        }

        // POST: ApplicationSecurityModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ApplicationName,SecurityRequirement,ComplianceStandards,Vulnerabilities,MitigationStrategies,Owner,LastAuditDate,Dependencies")] 
            ApplicationSecurityModel securityModel)
        {
            _logger.LogInformation("ApplicationSecurityController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != securityModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    securityModel.UserModified = User.Identity.Name;
                    securityModel.DateModified = DateTime.UtcNow;
                    
                    // Initialize collections if null
                    securityModel.ComplianceStandards ??= new List<string>();
                    securityModel.Vulnerabilities ??= new List<string>();
                    securityModel.MitigationStrategies ??= new List<string>();
                    securityModel.Dependencies ??= new List<string>();
                    
                    _context.Update(securityModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityModelExists(securityModel.Id))
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
            return View(securityModel);
        }

        // GET: ApplicationSecurityModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityModel = await _context.ApplicationSecurityModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityModel == null)
            {
                return NotFound();
            }

            return View(securityModel);
        }

        // POST: ApplicationSecurityModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var securityModel = await _context.ApplicationSecurityModels.FindAsync(id);
            _context.ApplicationSecurityModels.Remove(securityModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecurityModelExists(int id)
        {
            return _context.ApplicationSecurityModels.Any(e => e.Id == id);
        }
    }
}