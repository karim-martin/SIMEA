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
    public class ResourceNeedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourceNeedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ResourceNeedsModel
        public async Task<IActionResult> Index()
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Index at {DateTime.UtcNow}");
            return View(await _context.ResourceNeedsModels.ToListAsync());
        }

        // GET: ResourceNeedsModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Details at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var resourceNeed = await _context.ResourceNeedsModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resourceNeed == null)
            {
                return NotFound();
            }

            return View(resourceNeed);
        }

        // GET: ResourceNeedsModel/Create
        public IActionResult Create()
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Create at {DateTime.UtcNow}");
            return View();
        }

        // POST: ResourceNeedsModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResourceNeedsModel resourceNeeds)
        {
            if (ModelState.IsValid)
            {
                resourceNeeds.UserCreated = User.Identity.Name;
                resourceNeeds.DateCreated = DateTime.UtcNow;

                _context.Add(resourceNeeds);
                await _context.SaveChangesAsync();

                Console.WriteLine($"User {User.Identity.Name} created ResourceNeedsModel with ID {resourceNeeds.Id} at {DateTime.UtcNow}");

                return RedirectToAction(nameof(Index));
            }
            return View(resourceNeeds);
        }

        // GET: ResourceNeedsModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Edit at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var resourceNeed = await _context.ResourceNeedsModels.FindAsync(id);
            if (resourceNeed == null)
            {
                return NotFound();
            }
            return View(resourceNeed);
        }

        // POST: ResourceNeedsModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ResourceType,ResourceName,Description,Quantity,Owner,Cost,Dependencies,AllocationStatus,UserCreated,DateCreated")] 
            ResourceNeedsModel resourceNeed)
        {
            if (id != resourceNeed.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    resourceNeed.UserModified = User.Identity.Name;
                    resourceNeed.DateModified = DateTime.UtcNow;

                    _context.Update(resourceNeed);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"User {User.Identity.Name} edited ResourceNeedsModel with ID {resourceNeed.Id} at {DateTime.UtcNow}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourceNeedsExists(resourceNeed.Id))
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
            return View(resourceNeed);
        }

        // GET: ResourceNeedsModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Delete at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var resourceNeed = await _context.ResourceNeedsModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resourceNeed == null)
            {
                return NotFound();
            }

            return View(resourceNeed);
        }

        // POST: ResourceNeedsModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resourceNeed = await _context.ResourceNeedsModels.FindAsync(id);
            _context.ResourceNeedsModels.Remove(resourceNeed);
            await _context.SaveChangesAsync();

            Console.WriteLine($"User {User.Identity.Name} deleted ResourceNeedsModel with ID {id} at {DateTime.UtcNow}");

            return RedirectToAction(nameof(Index));
        }

        private bool ResourceNeedsExists(int id)
        {
            return _context.ResourceNeedsModels.Any(e => e.Id == id);
        }
    }
}