using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class BusinessStrategyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BusinessStrategyController> _logger;

        public BusinessStrategyController(ApplicationDbContext context, ILogger<BusinessStrategyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: BusinessStrategyView
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("BusinessStrategyController.Index called by {user}", User.Identity.Name);
            return View(await _context.BusinessStrategyViews.ToListAsync());
        }

        // GET: BusinessStrategyView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("BusinessStrategyController.Details called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var businessStrategy = await _context.BusinessStrategyViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessStrategy == null)
            {
                return NotFound();
            }

            return View(businessStrategy);
        }

        // GET: BusinessStrategyView/Create
        public IActionResult Create()
        {
            _logger.LogInformation("BusinessStrategyController.Create GET called by {user}", User.Identity.Name);
            return View();
        }

        // POST: BusinessStrategyView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessStrategyView businessStrategies)
        {
            _logger.LogInformation("BusinessStrategyController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                businessStrategies.UserCreated = User.Identity.Name;
                businessStrategies.DateCreated = DateTime.UtcNow;
                
                // Initialize collections if null
                businessStrategies.KeyPerformanceIndicators ??= new List<string>();
                businessStrategies.Stakeholders ??= new List<string>();
                businessStrategies.Risks ??= new List<string>();
                businessStrategies.Dependencies ??= new List<string>();
                
                _context.Add(businessStrategies);
                await _context.SaveChangesAsync();
                _logger.LogInformation("BusinessStrategy created with ID {id} by {user}", businessStrategies.Id, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(businessStrategies);
        }

        // GET: BusinessStrategyView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("BusinessStrategyController.Edit GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var businessStrategy = await _context.BusinessStrategyViews.FindAsync(id);
            if (businessStrategy == null)
            {
                return NotFound();
            }
            return View(businessStrategy);
        }

        // POST: BusinessStrategyView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,BusinessGoal,StrategicObjective,KeyPerformanceIndicators,Stakeholders,Timeframe,OrganizationalVisionAlignment,Risks,Dependencies")] 
            BusinessStrategyView businessStrategy)
        {
            _logger.LogInformation("BusinessStrategyController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != businessStrategy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    businessStrategy.UserModified = User.Identity.Name;
                    businessStrategy.DateModified = DateTime.UtcNow;
                    
                    // Initialize collections if null
                    businessStrategy.KeyPerformanceIndicators ??= new List<string>();
                    businessStrategy.Stakeholders ??= new List<string>();
                    businessStrategy.Risks ??= new List<string>();
                    businessStrategy.Dependencies ??= new List<string>();
                    
                    _context.Update(businessStrategy);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("BusinessStrategy updated with ID {id} by {user}", businessStrategy.Id, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessStrategyExists(businessStrategy.Id))
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
            return View(businessStrategy);
        }

        // GET: BusinessStrategyView/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("BusinessStrategyController.Delete GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var businessStrategy = await _context.BusinessStrategyViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessStrategy == null)
            {
                return NotFound();
            }

            return View(businessStrategy);
        }

        // POST: BusinessStrategyView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("BusinessStrategyController.DeleteConfirmed POST called for id {id} by {user}", id, User.Identity.Name);
            var businessStrategy = await _context.BusinessStrategyViews.FindAsync(id);
            _context.BusinessStrategyViews.Remove(businessStrategy);
            await _context.SaveChangesAsync();
            _logger.LogInformation("BusinessStrategy deleted with ID {id} by {user}", id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessStrategyExists(int id)
        {
            return _context.BusinessStrategyViews.Any(e => e.Id == id);
        }
    }
}