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
    public class DataFlowController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataFlowController> _logger;

        public DataFlowController(ApplicationDbContext context, ILogger<DataFlowController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DataFlowModel
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("DataFlowController.Index called by {user}", User.Identity.Name);
            return View(await _context.DataFlowModels.ToListAsync());
        }

        // GET: DataFlowModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("DataFlowController.Details called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataFlow = await _context.DataFlowModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataFlow == null)
            {
                return NotFound();
            }

            return View(dataFlow);
        }

        // GET: DataFlowModel/Create
        public IActionResult Create()
        {
            _logger.LogInformation("DataFlowController.Create GET called by {user}", User.Identity.Name);
            return View();
        }

        // POST: DataFlowModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DataFlowModel dataFlows)
        {
            _logger.LogInformation("DataFlowController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                dataFlows.UserCreated = User.Identity.Name;
                dataFlows.DateCreated = DateTime.UtcNow;
                
                // Initialize Dependencies collection if null
                dataFlows.Dependencies ??= new List<string>();
                
                _context.Add(dataFlows);
                await _context.SaveChangesAsync();
                _logger.LogInformation("DataFlow created with ID {id} by {user}", dataFlows.Id, User.Identity.Name);

                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Invalid model state in Create action at {Time}.", DateTime.UtcNow);
            return View(dataFlows);
        }

        // GET: DataFlowModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("DataFlowController.Edit GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataFlow = await _context.DataFlowModels.FindAsync(id);
            if (dataFlow == null)
            {
                return NotFound();
            }
            return View(dataFlow);
        }

        // POST: DataFlowModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,DataSource,DataDestination,DataFlowDescription,DataTypeName,Frequency,Owner,SecurityRequirements,Dependencies")] 
            DataFlowModel dataFlow)
        {
            _logger.LogInformation("DataFlowController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != dataFlow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dataFlow.UserModified = User.Identity.Name;
                    dataFlow.DateModified = DateTime.UtcNow;
                    
                    // Initialize Dependencies collection if null
                    dataFlow.Dependencies ??= new List<string>();
                    
                    _context.Update(dataFlow);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("DataFlow updated with ID {id} by {user}", dataFlow.Id, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataFlowExists(dataFlow.Id))
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
            return View(dataFlow);
        }

        // GET: DataFlowModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("DataFlowController.Delete GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var dataFlow = await _context.DataFlowModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataFlow == null)
            {
                return NotFound();
            }

            return View(dataFlow);
        }

        // POST: DataFlowModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("DataFlowController.DeleteConfirmed POST called for id {id} by {user}", id, User.Identity.Name);
            var dataFlow = await _context.DataFlowModels.FindAsync(id);
            _context.DataFlowModels.Remove(dataFlow);
            await _context.SaveChangesAsync();
            _logger.LogInformation("DataFlow deleted with ID {id} by {user}", id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool DataFlowExists(int id)
        {
            return _context.DataFlowModels.Any(e => e.Id == id);
        }
    }
}