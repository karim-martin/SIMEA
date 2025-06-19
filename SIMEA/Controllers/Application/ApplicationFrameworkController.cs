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
    public class ApplicationFrameworkController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationFrameworkController> _logger;

        public ApplicationFrameworkController(ApplicationDbContext context, ILogger<ApplicationFrameworkController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ApplicationArchitectureFramework
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ApplicationFrameworkController.Index called by {user}", User.Identity.Name);
            return View(await _context.ApplicationArchitectureFrameworks.ToListAsync());
        }

        // GET: ApplicationArchitectureFramework/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var framework = await _context.ApplicationArchitectureFrameworks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (framework == null)
            {
                return NotFound();
            }

            return View(framework);
        }

        // GET: ApplicationArchitectureFramework/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationArchitectureFramework/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationArchitectureFramework frameworks)
        {
            _logger.LogInformation("ApplicationFrameworkController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                frameworks.UserCreated = User.Identity.Name;
                frameworks.DateCreated = DateTime.UtcNow;
                
                // Initialize collections if null
                frameworks.TechnologyStack ??= new List<string>();
                frameworks.IntegrationPoints ??= new List<string>();
                frameworks.RedundantWith ??= new List<string>();
                frameworks.RedundantApplications ??= new List<string>();
                frameworks.ReplacementOptions ??= new List<string>();
                frameworks.Recommendations ??= new List<ApplicationRecommendation>();
                
                _context.Add(frameworks);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(frameworks);
        }

        // GET: ApplicationArchitectureFramework/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var framework = await _context.ApplicationArchitectureFrameworks.FindAsync(id);
            if (framework == null)
            {
                return NotFound();
            }
            return View(framework);
        }

        // POST: ApplicationArchitectureFramework/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ApplicationName,ApplicationDescription,BusinessFunctionSupported,TechnologyStack,IntegrationPoints,Owner,Version,DeploymentEnvironment,RationalizationCategory,BusinessValue,TechnicalFit,AnnualCost,RiskScore,CurrentState,FutureState,RationalizationRationale,RedundantApplications,ReplacementOptions,AssessmentDate,Recommendations")] 
            ApplicationArchitectureFramework framework)
        {
            _logger.LogInformation("ApplicationFrameworkController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != framework.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    framework.UserModified = User.Identity.Name;
                    framework.DateModified = DateTime.UtcNow;
                    
                    // Initialize collections if null
                    framework.TechnologyStack ??= new List<string>();
                    framework.IntegrationPoints ??= new List<string>();
                    framework.RedundantWith ??= new List<string>();
                    framework.RedundantApplications ??= new List<string>();
                    framework.ReplacementOptions ??= new List<string>();
                    framework.Recommendations ??= new List<ApplicationRecommendation>();
                    
                    _context.Update(framework);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FrameworkExists(framework.Id))
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
            return View(framework);
        }

        // GET: ApplicationArchitectureFramework/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var framework = await _context.ApplicationArchitectureFrameworks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (framework == null)
            {
                return NotFound();
            }

            return View(framework);
        }

        // POST: ApplicationArchitectureFramework/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var framework = await _context.ApplicationArchitectureFrameworks.FindAsync(id);
            _context.ApplicationArchitectureFrameworks.Remove(framework);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FrameworkExists(int id)
        {
            return _context.ApplicationArchitectureFrameworks.Any(e => e.Id == id);
        }
    }
}