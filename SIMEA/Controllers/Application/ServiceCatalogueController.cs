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
    public class ServiceCatalogueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceCatalogueController> _logger;

        public ServiceCatalogueController(ApplicationDbContext context, ILogger<ServiceCatalogueController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ServiceCatalogue
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ServiceCatalogueController.Index called by {user}", User.Identity.Name);
            return View(await _context.ServiceCatalogues.ToListAsync());
        }

        // GET: ServiceCatalogue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceCatalogue = await _context.ServiceCatalogues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceCatalogue == null)
            {
                return NotFound();
            }

            return View(serviceCatalogue);
        }

        // GET: ServiceCatalogue/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceCatalogue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCatalogue services)
        {
            _logger.LogInformation("ServiceCatalogueController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                services.UserCreated = User.Identity.Name;
                services.DateCreated = DateTime.UtcNow;
                _context.Add(services);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(services);
        }

        // GET: ServiceCatalogue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceCatalogue = await _context.ServiceCatalogues.FindAsync(id);
            if (serviceCatalogue == null)
            {
                return NotFound();
            }
            return View(serviceCatalogue);
        }

        // POST: ServiceCatalogue/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ServiceName,ServiceDescription,ServiceOwner,SLA,SupportedApplications,Availability,Cost,Dependencies,UserCreated,DateCreated")] 
            ServiceCatalogue serviceCatalogue)
        {
            _logger.LogInformation("ServiceCatalogueController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != serviceCatalogue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    serviceCatalogue.UserModified = User.Identity.Name;
                    serviceCatalogue.DateModified = DateTime.UtcNow;
                    _context.Update(serviceCatalogue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceCatalogueExists(serviceCatalogue.Id))
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
            return View(serviceCatalogue);
        }

        // GET: ServiceCatalogue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceCatalogue = await _context.ServiceCatalogues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceCatalogue == null)
            {
                return NotFound();
            }

            return View(serviceCatalogue);
        }

        // POST: ServiceCatalogue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceCatalogue = await _context.ServiceCatalogues.FindAsync(id);
            _context.ServiceCatalogues.Remove(serviceCatalogue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceCatalogueExists(int id)
        {
            return _context.ServiceCatalogues.Any(e => e.Id == id);
        }
    }
}