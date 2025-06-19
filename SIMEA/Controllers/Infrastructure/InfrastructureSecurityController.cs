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
    public class InfrastructureSecurityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InfrastructureSecurityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InfrastructureSecurityModel
        public async Task<IActionResult> Index()
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Index at {DateTime.UtcNow}");
            return View(await _context.InfrastructureSecurityModels.ToListAsync());
        }

        // GET: InfrastructureSecurityModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Details at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var securityModel = await _context.InfrastructureSecurityModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityModel == null)
            {
                return NotFound();
            }

            return View(securityModel);
        }

        // GET: InfrastructureSecurityModel/Create
        public IActionResult Create()
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Create at {DateTime.UtcNow}");
            return View();
        }

        // POST: InfrastructureSecurityModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InfrastructureSecurityModel securityModels)
        {
            if (ModelState.IsValid)
            {
                // Initialize collections if they are null
                securityModels.ComplianceStandards ??= new List<string>();
                securityModels.Vulnerabilities ??= new List<string>();
                securityModels.MitigationStrategies ??= new List<string>();
                securityModels.Dependencies ??= new List<string>();
                
                securityModels.UserCreated = User.Identity.Name;
                securityModels.DateCreated = DateTime.UtcNow;

                _context.Add(securityModels);
                await _context.SaveChangesAsync();

                Console.WriteLine($"User {User.Identity.Name} created InfrastructureSecurityModel with ID {securityModels.Id} at {DateTime.UtcNow}");

                return RedirectToAction(nameof(Index));
            }
            return View(securityModels);
        }
        
        // GET: InfrastructureSecurityModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine($"User {User.Identity.Name} accessed Edit at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var securityModel = await _context.InfrastructureSecurityModels.FindAsync(id);
            if (securityModel == null)
            {
                return NotFound();
            }
            return View(securityModel);
        }

        // POST: InfrastructureSecurityModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,SystemName,SecurityRequirement,ComplianceStandards,Vulnerabilities,MitigationStrategies,Owner,LastAuditDate,Dependencies")] 
            InfrastructureSecurityModel securityModel)
        {
            if (id != securityModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingModel = await _context.InfrastructureSecurityModels.AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id == id);
                    
                    if (existingModel == null)
                    {
                        return NotFound();
                    }
                    
                    securityModel.UserCreated = existingModel.UserCreated;
                    securityModel.DateCreated = existingModel.DateCreated;
                    
                    securityModel.UserModified = User.Identity.Name;
                    securityModel.DateModified = DateTime.UtcNow;

                    _context.Update(securityModel);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"User {User.Identity.Name} edited InfrastructureSecurityModel with ID {securityModel.Id} at {DateTime.UtcNow}");
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

        // GET: InfrastructureSecurityModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {   
            Console.WriteLine($"User {User.Identity.Name} accessed Delete at {DateTime.UtcNow}");
            if (id == null)
            {
                return NotFound();
            }

            var securityModel = await _context.InfrastructureSecurityModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityModel == null)
            {
                return NotFound();
            }

            return View(securityModel);
        }

        // POST: InfrastructureSecurityModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var securityModel = await _context.InfrastructureSecurityModels.FindAsync(id);
            _context.InfrastructureSecurityModels.Remove(securityModel);
            await _context.SaveChangesAsync();

            Console.WriteLine($"User {User.Identity.Name} deleted InfrastructureSecurityModel with ID {id} at {DateTime.UtcNow}");

            return RedirectToAction(nameof(Index));
        }

        private bool SecurityModelExists(int id)
        {
            return _context.InfrastructureSecurityModels.Any(e => e.Id == id);
        }
    }
}