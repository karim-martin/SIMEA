using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class InfrastructureBusinessRequirementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InfrastructureBusinessRequirementsController> _logger;

        public InfrastructureBusinessRequirementsController(ApplicationDbContext context, ILogger<InfrastructureBusinessRequirementsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: InfrastructureBusinessRequirementsView
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("User {User} is accessing InfrastructureBusinessRequirements Index at {Time}.", User.Identity.Name, DateTime.UtcNow);
            return View(await _context.InfrastructureBusinessRequirementsViews.ToListAsync());
        }

        // GET: InfrastructureBusinessRequirementsView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details request with null ID at {Time}.", DateTime.UtcNow);
                return NotFound();
            }

            var requirement = await _context.InfrastructureBusinessRequirementsViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requirement == null)
            {
                _logger.LogWarning("Requirement with ID {Id} not found at {Time}.", id, DateTime.UtcNow);
                return NotFound();
            }

            _logger.LogInformation("User {User} is accessing InfrastructureBusinessRequirements Details for ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
            return View(requirement);
        }

        // GET: InfrastructureBusinessRequirementsView/Create
        public IActionResult Create()
        {
            _logger.LogInformation("User {User} is accessing InfrastructureBusinessRequirements Create at {Time}.", User.Identity.Name, DateTime.UtcNow);
            return View();
        }

        // POST: InfrastructureBusinessRequirementsView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InfrastructureBusinessRequirementsView requirements)
        {
            if (ModelState.IsValid)
            {
                // Initialize collections if they are null
                requirements.Stakeholders ??= new List<string>();
                
                requirements.UserCreated = User.Identity.Name;
                requirements.DateCreated = DateTime.UtcNow;
                _context.Add(requirements);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User {User} created InfrastructureBusinessRequirements with ID {Id} at {Time}.", User.Identity.Name, requirements.Id, DateTime.UtcNow);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Invalid model state in Create action at {Time}.", DateTime.UtcNow);
            return View(requirements);
        }

        // GET: InfrastructureBusinessRequirementsView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit request with null ID at {Time}.", DateTime.UtcNow);
                return NotFound();
            }

            var requirement = await _context.InfrastructureBusinessRequirementsViews.FindAsync(id);
            if (requirement == null)
            {
                _logger.LogWarning("Requirement with ID {Id} not found for editing at {Time}.", id, DateTime.UtcNow);
                return NotFound();
            }

            _logger.LogInformation("User {User} is accessing InfrastructureBusinessRequirements Edit for ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
            return View(requirement);
        }

        // POST: InfrastructureBusinessRequirementsView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,RequirementId,RequirementDescription,BusinessUnit,Priority,Stakeholders,Status,AssignedTo,DueDate")] 
            InfrastructureBusinessRequirementsView requirement)
        {
            if (id != requirement.Id)
            {
                _logger.LogWarning("Edit request with ID mismatch at {Time}.", DateTime.UtcNow);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {       
                    var existingRequirement = await _context.InfrastructureBusinessRequirementsViews.AsNoTracking()
                        .FirstOrDefaultAsync(r => r.Id == id);
                    
                    if (existingRequirement == null)
                    {
                        _logger.LogWarning("Requirement with ID {Id} not found at {Time}.", id, DateTime.UtcNow);
                        return NotFound();
                    }
                    

                    requirement.UserCreated = existingRequirement.UserCreated;
                    requirement.DateCreated = existingRequirement.DateCreated;
                    requirement.UserModified = User.Identity.Name;
                    requirement.DateModified = DateTime.UtcNow;
                    
                    _context.Update(requirement);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User {User} updated InfrastructureBusinessRequirements with ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequirementExists(requirement.Id))
                    {
                        _logger.LogWarning("Concurrency exception: Requirement with ID {Id} not found at {Time}.", requirement.Id, DateTime.UtcNow);
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Invalid model state in Edit action at {Time}.", DateTime.UtcNow);
            return View(requirement);
        }

        // GET: InfrastructureBusinessRequirementsView/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete request with null ID at {Time}.", DateTime.UtcNow);
                return NotFound();
            }

            var requirement = await _context.InfrastructureBusinessRequirementsViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requirement == null)
            {
                _logger.LogWarning("Requirement with ID {Id} not found for deletion at {Time}.", id, DateTime.UtcNow);
                return NotFound();
            }

            _logger.LogInformation("User {User} is accessing InfrastructureBusinessRequirements Delete for ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
            return View(requirement);
        }

        // POST: InfrastructureBusinessRequirementsView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requirement = await _context.InfrastructureBusinessRequirementsViews.FindAsync(id);
            _context.InfrastructureBusinessRequirementsViews.Remove(requirement);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {User} deleted InfrastructureBusinessRequirements with ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }

        private bool RequirementExists(int id)
        {
            return _context.InfrastructureBusinessRequirementsViews.Any(e => e.Id == id);
        }
    }
}