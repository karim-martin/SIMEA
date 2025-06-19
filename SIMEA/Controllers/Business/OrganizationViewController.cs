using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class OrganizationViewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrganizationViewController> _logger;

        public OrganizationViewController(ApplicationDbContext context, ILogger<OrganizationViewController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: OrganizationView
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("OrganizationViewController.Index called by {user}", User.Identity.Name);
            return View(await _context.OrganizationViews.ToListAsync());
        }

        // GET: OrganizationView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("OrganizationViewController.Details called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.OrganizationViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // GET: OrganizationView/Create
        public IActionResult Create()
        {
            _logger.LogInformation("OrganizationViewController.Create GET called by {user}", User.Identity.Name);
            return View();
        }

        // POST: OrganizationView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationView organization)
        {
            _logger.LogInformation("OrganizationViewController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                organization.UserCreated = User.Identity.Name;
                organization.DateCreated = DateTime.UtcNow;
                _context.Add(organization);
                await _context.SaveChangesAsync();
                _logger.LogInformation("OrganizationView created with ID {id} by {user}", organization.Id, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: OrganizationView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("OrganizationViewController.Edit GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.OrganizationViews.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // POST: OrganizationView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,DepartmentOrUnit,RolesAndResponsibilities,ReportingStructure,KeyPersonnel,CollaborationPoints,OrganizationalGoals,Challenges,UserCreated,DateCreated")] 
            OrganizationView organization)
        {
            _logger.LogInformation("OrganizationViewController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != organization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    organization.UserModified = User.Identity.Name;
                    organization.DateModified = DateTime.UtcNow;
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("OrganizationView updated with ID {id} by {user}", organization.Id, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
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
            return View(organization);
        }

        // GET: OrganizationView/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("OrganizationViewController.Delete GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var organization = await _context.OrganizationViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // POST: OrganizationView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("OrganizationViewController.DeleteConfirmed POST called for id {id} by {user}", id, User.Identity.Name);
            var organization = await _context.OrganizationViews.FindAsync(id);
            _context.OrganizationViews.Remove(organization);
            await _context.SaveChangesAsync();
            _logger.LogInformation("OrganizationView deleted with ID {id} by {user}", id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
            return _context.OrganizationViews.Any(e => e.Id == id);
        }
    }
}