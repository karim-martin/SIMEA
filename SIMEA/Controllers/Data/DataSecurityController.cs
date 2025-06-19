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
    public class DataSecurityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataSecurityController> _logger;

        public DataSecurityController(ApplicationDbContext context, ILogger<DataSecurityController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DataSecurityModel
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("DataSecurityController.Index called by {user}", User.Identity.Name);
            return View(await _context.DataSecurityModels.ToListAsync());
        }

        // GET: DataSecurityModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("DataSecurityController.Details called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataSecurity = await _context.DataSecurityModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataSecurity == null)
            {
                return NotFound();
            }

            return View(dataSecurity);
        }

        // GET: DataSecurityModel/Create
        public IActionResult Create()
        {
            _logger.LogInformation("DataSecurityController.Create GET called by {user}", User.Identity.Name);
            return View();
        }

         // POST: DataSecurityModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DataSecurityModel dataSecurities)
        {
            _logger.LogInformation("DataSecurityController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                dataSecurities.UserCreated = User.Identity.Name;
                dataSecurities.DateCreated = DateTime.UtcNow;
                
                // Initialize collections if null
                dataSecurities.ComplianceStandards ??= new List<string>();
                dataSecurities.Dependencies ??= new List<string>();
                
                _context.Add(dataSecurities);
                await _context.SaveChangesAsync();
                _logger.LogInformation("DataSecurity created with ID {id} by {user}", dataSecurities.Id, User.Identity.Name);

                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Invalid model state in Create action at {Time}.", DateTime.UtcNow);
            return View(dataSecurities);
        }

        // GET: DataSecurityModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("DataSecurityController.Edit GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataSecurity = await _context.DataSecurityModels.FindAsync(id);
            if (dataSecurity == null)
            {
                return NotFound();
            }
            return View(dataSecurity);
        }

        // POST: DataSecurityModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,DataEntity,SecurityRequirement,AccessControls,EncryptionRequirements,ComplianceStandards,Owner,LastAuditDate,Dependencies")] 
            DataSecurityModel dataSecurity)
        {
            _logger.LogInformation("DataSecurityController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != dataSecurity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dataSecurity.UserModified = User.Identity.Name;
                    dataSecurity.DateModified = DateTime.UtcNow;
                    
                    // Initialize collections if null
                    dataSecurity.ComplianceStandards ??= new List<string>();
                    dataSecurity.Dependencies ??= new List<string>();
                    
                    _context.Update(dataSecurity);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("DataSecurity updated with ID {id} by {user}", dataSecurity.Id, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataSecurityExists(dataSecurity.Id))
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
            return View(dataSecurity);
        }

        // GET: DataSecurityModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("DataSecurityController.Delete GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataSecurity = await _context.DataSecurityModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataSecurity == null)
            {
                return NotFound();
            }

            return View(dataSecurity);
        }

        // POST: DataSecurityModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("DataSecurityController.DeleteConfirmed POST called for id {id} by {user}", id, User.Identity.Name);
            var dataSecurity = await _context.DataSecurityModels.FindAsync(id);
            _context.DataSecurityModels.Remove(dataSecurity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("DataSecurity deleted with ID {id} by {user}", id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool DataSecurityExists(int id)
        {
            return _context.DataSecurityModels.Any(e => e.Id == id);
        }
    }
}