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
    public class ApplicationBusinessRequirementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationBusinessRequirementController> _logger;

        public ApplicationBusinessRequirementController(ApplicationDbContext context, ILogger<ApplicationBusinessRequirementController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ApplicationBusinessRequirementsView
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ApplicationBusinessRequirementController.Index called by {user}", User.Identity.Name);
            return View(await _context.ApplicationBusinessRequirementsViews.ToListAsync());
        }

        // GET: ApplicationBusinessRequirementsView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.ApplicationBusinessRequirementsViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requirement == null)
            {
                return NotFound();
            }

            return View(requirement);
        }

        // GET: ApplicationBusinessRequirementsView/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Asset/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationBusinessRequirementsView requirements)
        {
            _logger.LogInformation("ApplicationBusinessRequirementController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                requirements.UserCreated = User.Identity.Name;
                requirements.DateCreated = DateTime.UtcNow;
                
                // Initialize collections if null
                requirements.Stakeholders ??= new List<string>();
                requirements.Dependencies ??= new List<string>();
                
                _context.Add(requirements);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(requirements);
        }

        // GET: ApplicationBusinessRequirementsView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.ApplicationBusinessRequirementsViews.FindAsync(id);
            if (requirement == null)
            {
                return NotFound();
            }
            return View(requirement);
        }

        // POST: ApplicationBusinessRequirementsView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,RequirementId,RequirementDescription,Priority,Stakeholders,Status,AssignedTo,DueDate,Dependencies")] 
            ApplicationBusinessRequirementsView requirement)
        {
            _logger.LogInformation("ApplicationBusinessRequirementController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != requirement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    requirement.UserModified = User.Identity.Name;
                    requirement.DateModified = DateTime.UtcNow;
                    
                    // Initialize collections if null
                    requirement.Stakeholders ??= new List<string>();
                    requirement.Dependencies ??= new List<string>();
                    
                    _context.Update(requirement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequirementExists(requirement.Id))
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
            return View(requirement);
        }

        // GET: ApplicationBusinessRequirementsView/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.ApplicationBusinessRequirementsViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requirement == null)
            {
                return NotFound();
            }

            return View(requirement);
        }

        // POST: ApplicationBusinessRequirementsView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requirement = await _context.ApplicationBusinessRequirementsViews.FindAsync(id);
            _context.ApplicationBusinessRequirementsViews.Remove(requirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequirementExists(int id)
        {
            return _context.ApplicationBusinessRequirementsViews.Any(e => e.Id == id);
        }
    }
}