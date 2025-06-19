using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class InformationRequirementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InformationRequirementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InformationRequirementsView
        public async Task<IActionResult> Index()
        {
            return View(await _context.InformationRequirementsViews.ToListAsync());
        }

        // GET: InformationRequirementsView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.InformationRequirementsViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requirement == null)
            {
                return NotFound();
            }

            return View(requirement);
        }

        // GET: InformationRequirementsView/Create
        public IActionResult Create()
        {
            return View();
        }

         // POST: InformationRequirementsView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InformationRequirementsView requirements)
        {
            if (ModelState.IsValid)
            {
                // Initialize collections if they are null
                requirements.Stakeholders ??= new List<string>();
                
                requirements.UserCreated = User.Identity.Name;
                requirements.DateCreated = DateTime.UtcNow;
                _context.Add(requirements);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(requirements);
        }

        // GET: InformationRequirementsView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.InformationRequirementsViews.FindAsync(id);
            if (requirement == null)
            {
                return NotFound();
            }
            return View(requirement);
        }

        // POST: InformationRequirementsView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,RequirementId,RequirementDescription,DataEntity,Priority,Stakeholders,Status,AssignedTo,DueDate")] 
            InformationRequirementsView requirement)
        {
            if (id != requirement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing record to preserve creation audit data
                    var existingRequirement = await _context.InformationRequirementsViews.AsNoTracking()
                        .FirstOrDefaultAsync(r => r.Id == id);
                    
                    if (existingRequirement == null)
                    {
                        return NotFound();
                    }
                    
                    // Preserve creation audit data
                    requirement.UserCreated = existingRequirement.UserCreated;
                    requirement.DateCreated = existingRequirement.DateCreated;
                    
                    // Update modification audit data
                    requirement.UserModified = User.Identity.Name;
                    requirement.DateModified = DateTime.UtcNow;
                    
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

        // GET: InformationRequirementsView/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requirement = await _context.InformationRequirementsViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requirement == null)
            {
                return NotFound();
            }

            return View(requirement);
        }

        // POST: InformationRequirementsView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requirement = await _context.InformationRequirementsViews.FindAsync(id);
            _context.InformationRequirementsViews.Remove(requirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequirementExists(int id)
        {
            return _context.InformationRequirementsViews.Any(e => e.Id == id);
        }
    }
}