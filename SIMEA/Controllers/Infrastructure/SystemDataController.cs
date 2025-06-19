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
    public class SystemDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SystemDataCrossReference
        public async Task<IActionResult> Index()
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Index at {DateTime.UtcNow}");
            return View(await _context.SystemDataCrossReferences.ToListAsync());
        }

        // GET: SystemDataCrossReference/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Details at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.SystemDataCrossReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossRef == null)
            {
                return NotFound();
            }

            return View(crossRef);
        }

        // GET: SystemDataCrossReference/Create
        public IActionResult Create()
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Create at {DateTime.UtcNow}");
            return View();
        }

        // POST: SystemDataCrossReference/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemDataCrossReference crossRefs)
        {
            if (ModelState.IsValid)
            {
                crossRefs.UserCreated = User.Identity.Name;
                crossRefs.DateCreated = DateTime.UtcNow;

                _context.Add(crossRefs);
                await _context.SaveChangesAsync();

                Console.WriteLine($"User {User.Identity.Name} created SystemDataCrossReference with ID {crossRefs.Id} at {DateTime.UtcNow}");

                return RedirectToAction(nameof(Index));
            }
            return View(crossRefs);
        }

        // GET: SystemDataCrossReference/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Log the action
            Console.WriteLine($"User {User.Identity.Name} accessed Edit at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.SystemDataCrossReferences.FindAsync(id);
            if (crossRef == null)
            {
                return NotFound();
            }
            return View(crossRef);
        }

        // POST: SystemDataCrossReference/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,SystemName,DataEntity,DataUsage,DataSensitivity,AccessControls,Owner,DataRetentionPolicy,Dependencies")] 
            SystemDataCrossReference crossRef)
        {
            if (id != crossRef.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCrossRef = await _context.SystemDataCrossReferences.AsNoTracking()
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

                    Console.WriteLine($"User {User.Identity.Name} edited SystemDataCrossReference with ID {crossRef.Id} at {DateTime.UtcNow}");
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

        // GET: SystemDataCrossReference/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Delete at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var crossRef = await _context.SystemDataCrossReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crossRef == null)
            {
                return NotFound();
            }

            return View(crossRef);
        }

        // POST: SystemDataCrossReference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crossRef = await _context.SystemDataCrossReferences.FindAsync(id);
            if (crossRef != null)
            {
                _context.SystemDataCrossReferences.Remove(crossRef);
                await _context.SaveChangesAsync();

                Console.WriteLine($"User {User.Identity.Name} deleted SystemDataCrossReference with ID {id} at {DateTime.UtcNow}");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CrossRefExists(int id)
        {
            return _context.SystemDataCrossReferences.Any(e => e.Id == id);
        }
    }
}