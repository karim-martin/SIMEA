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
    public class SystemToApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemToApplicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SystemToApplicationCrossReference
        public async Task<IActionResult> Index()
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Index at {DateTime.UtcNow}");
            return View(await _context.SystemToApplicationCrossReferences.ToListAsync());
        }

        // GET: SystemToApplicationCrossReference/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Details at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.SystemToApplicationCrossReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossRef == null)
            {
                return NotFound();
            }

            return View(crossRef);
        }

        // GET: SystemToApplicationCrossReference/Create
        public IActionResult Create()
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Create at {DateTime.UtcNow}");
            return View();
        }

        // POST: SystemToApplicationCrossReference/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemToApplicationCrossReference crossRefs)
        {
            if (ModelState.IsValid)
            {
                crossRefs.UserCreated = User.Identity.Name;
                crossRefs.DateCreated = DateTime.UtcNow;

                _context.Add(crossRefs);
                await _context.SaveChangesAsync();

                Console.WriteLine($"User {User.Identity.Name} created SystemToApplicationCrossReference with ID {crossRefs.Id} at {DateTime.UtcNow}");

                return RedirectToAction(nameof(Index));
            }
            return View(crossRefs);
        }

        // GET: SystemToApplicationCrossReference/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Edit at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.SystemToApplicationCrossReferences.FindAsync(id);
            if (crossRef == null)
            {
                return NotFound();
            }
            return View(crossRef);
        }

        // POST: SystemToApplicationCrossReference/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,SystemName,ApplicationName,IntegrationType,DataExchanged,Frequency,Owner,SecurityRequirements,Dependencies")] 
            SystemToApplicationCrossReference crossRef)
        {
            if (id != crossRef.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCrossRef = await _context.SystemToApplicationCrossReferences.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);
                    if (existingCrossRef != null)
                    {
                        crossRef.UserCreated = existingCrossRef.UserCreated;
                        crossRef.DateCreated = existingCrossRef.DateCreated;
                    }
                    
                    crossRef.UserModified = User.Identity.Name;
                    crossRef.DateModified = DateTime.UtcNow;

                    _context.Update(crossRef);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"User {User.Identity.Name} edited SystemToApplicationCrossReference with ID {crossRef.Id} at {DateTime.UtcNow}");
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

        // GET: SystemToApplicationCrossReference/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {       
            Console.WriteLine($"User {User.Identity.Name} accessed Delete at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.SystemToApplicationCrossReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossRef == null)
            {
                return NotFound();
            }

            return View(crossRef);
        }

        // POST: SystemToApplicationCrossReference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crossRef = await _context.SystemToApplicationCrossReferences.FindAsync(id);
            if (crossRef != null)
            {
                _context.SystemToApplicationCrossReferences.Remove(crossRef);
                await _context.SaveChangesAsync();

                Console.WriteLine($"User {User.Identity.Name} deleted SystemToApplicationCrossReference with ID {id} at {DateTime.UtcNow}");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CrossRefExists(int id)
        {
            return _context.SystemToApplicationCrossReferences.Any(e => e.Id == id);
        }
    }
}