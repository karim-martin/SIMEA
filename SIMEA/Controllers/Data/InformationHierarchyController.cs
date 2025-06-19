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
    public class InformationHierarchyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InformationHierarchyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InformationHierarchyView
        public async Task<IActionResult> Index()
        {
            return View(await _context.InformationHierarchyViews.ToListAsync());
        }

        // GET: InformationHierarchyView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hierarchy = await _context.InformationHierarchyViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hierarchy == null)
            {
                return NotFound();
            }

            return View(hierarchy);
        }

        // GET: InformationHierarchyView/Create
        public IActionResult Create()
        {
            return View();
        }

         // POST: InformationHierarchyView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InformationHierarchyView hierarchies)
        {
            if (ModelState.IsValid)
            {
                // Initialize collections if they are null
                hierarchies.ChildInformation ??= new List<string>();
                hierarchies.Dependencies ??= new List<string>();
                
                hierarchies.UserCreated = User.Identity.Name;
                hierarchies.DateCreated = DateTime.UtcNow;
                _context.Add(hierarchies);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(hierarchies);
        }

        // GET: InformationHierarchyView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hierarchy = await _context.InformationHierarchyViews.FindAsync(id);
            if (hierarchy == null)
            {
                return NotFound();
            }
            return View(hierarchy);
        }

        // POST: InformationHierarchyView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,InformationLevel,ParentInformation,ChildInformation,Description,Owner,Usage,Dependencies,SecurityLevel")] 
            InformationHierarchyView hierarchy)
        {
            if (id != hierarchy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing record to preserve creation audit data
                    var existingHierarchy = await _context.InformationHierarchyViews.AsNoTracking()
                        .FirstOrDefaultAsync(h => h.Id == id);
                    
                    if (existingHierarchy == null)
                    {
                        return NotFound();
                    }
                    
                    // Preserve creation audit data
                    hierarchy.UserCreated = existingHierarchy.UserCreated;
                    hierarchy.DateCreated = existingHierarchy.DateCreated;
                    
                    // Update modification audit data
                    hierarchy.UserModified = User.Identity.Name;
                    hierarchy.DateModified = DateTime.UtcNow;
                    
                    _context.Update(hierarchy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HierarchyExists(hierarchy.Id))
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
            return View(hierarchy);
        }

        // GET: InformationHierarchyView/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hierarchy = await _context.InformationHierarchyViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hierarchy == null)
            {
                return NotFound();
            }

            return View(hierarchy);
        }

        // POST: InformationHierarchyView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hierarchy = await _context.InformationHierarchyViews.FindAsync(id);
            _context.InformationHierarchyViews.Remove(hierarchy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HierarchyExists(int id)
        {
            return _context.InformationHierarchyViews.Any(e => e.Id == id);
        }
    }
}