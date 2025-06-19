using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class DataGovernanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataGovernanceController> _logger;

        public DataGovernanceController(ApplicationDbContext context, ILogger<DataGovernanceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DataGovernanceModel
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("DataGovernanceController.Index called by {user}", User.Identity.Name);
            return View(await _context.DataGovernanceModels.ToListAsync());
        }

        // GET: DataGovernanceModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("DataGovernanceController.Details called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataGovernance = await _context.DataGovernanceModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataGovernance == null)
            {
                return NotFound();
            }

            return View(dataGovernance);
        }

        // GET: DataGovernanceModel/Create
        public IActionResult Create()
        {
            _logger.LogInformation("DataGovernanceController.Create GET called by {user}", User.Identity.Name);
            return View();
        }

         // POST: DataGovernanceModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DataGovernanceModel dataGovernances)
        {
            _logger.LogInformation("DataGovernanceController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                dataGovernances.UserCreated = User.Identity.Name;
                dataGovernances.DateCreated = DateTime.UtcNow;
                
                // Initialize collections if null
                dataGovernances.DataQualityMetrics ??= new List<string>();
                dataGovernances.DataPolicies ??= new List<string>();
                dataGovernances.ComplianceRequirements ??= new List<string>();
                dataGovernances.Dependencies ??= new List<string>();
                
                _context.Add(dataGovernances);
                await _context.SaveChangesAsync();
                _logger.LogInformation("DataGovernance created with ID {id} by {user}", dataGovernances.Id, User.Identity.Name);

                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Invalid model state in Create action at {Time}.", DateTime.UtcNow);
            return View(dataGovernances);
        }
        
        // GET: DataGovernanceModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("DataGovernanceController.Edit GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataGovernance = await _context.DataGovernanceModels.FindAsync(id);
            if (dataGovernance == null)
            {
                return NotFound();
            }
            return View(dataGovernance);
        }

        // POST: DataGovernanceModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,DataEntity,DataOwner,DataSteward,DataQualityMetrics,DataPolicies,ComplianceRequirements,DataLifecycle,Dependencies")] 
            DataGovernanceModel dataGovernance)
        {
            _logger.LogInformation("DataGovernanceController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != dataGovernance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dataGovernance.UserModified = User.Identity.Name;
                    dataGovernance.DateModified = DateTime.UtcNow;
                    
                    // Initialize collections if null
                    dataGovernance.DataQualityMetrics ??= new List<string>();
                    dataGovernance.DataPolicies ??= new List<string>();
                    dataGovernance.ComplianceRequirements ??= new List<string>();
                    dataGovernance.Dependencies ??= new List<string>();
                    
                    _context.Update(dataGovernance);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("DataGovernance updated with ID {id} by {user}", dataGovernance.Id, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataGovernanceExists(dataGovernance.Id))
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
            _logger.LogWarning("Invalid model state in Edit action for id {id} at {Time}.", id, DateTime.UtcNow);
            return View(dataGovernance);
        }

        // GET: DataGovernanceModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("DataGovernanceController.Delete GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataGovernance = await _context.DataGovernanceModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataGovernance == null)
            {
                return NotFound();
            }

            return View(dataGovernance);
        }

        // POST: DataGovernanceModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("DataGovernanceController.DeleteConfirmed POST called for id {id} by {user}", id, User.Identity.Name);
            var dataGovernance = await _context.DataGovernanceModels.FindAsync(id);
            _context.DataGovernanceModels.Remove(dataGovernance);
            await _context.SaveChangesAsync();
            _logger.LogInformation("DataGovernance deleted with ID {id} by {user}", id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool DataGovernanceExists(int id)
        {
            return _context.DataGovernanceModels.Any(e => e.Id == id);
        }
    }
}