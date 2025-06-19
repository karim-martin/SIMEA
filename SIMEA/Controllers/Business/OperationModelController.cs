using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class OperationModelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OperationModelController> _logger;

        public OperationModelController(ApplicationDbContext context, ILogger<OperationModelController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: OperationModel
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("OperationModelController.Index called by {user}", User.Identity.Name);
            return View(await _context.OperationModels.ToListAsync());
        }

        // GET: OperationModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("OperationModelController.Details called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var operationModel = await _context.OperationModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operationModel == null)
            {
                return NotFound();
            }

            return View(operationModel);
        }

        // GET: OperationModel/Create
        public IActionResult Create()
        {
            _logger.LogInformation("OperationModelController.Create GET called by {user}", User.Identity.Name);
            return View();
        }

        // POST: OperationModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OperationModel operationModel)
        {
            _logger.LogInformation("OperationModelController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                operationModel.UserCreated = User.Identity.Name;
                operationModel.DateCreated = DateTime.UtcNow;
                _context.Add(operationModel);
                await _context.SaveChangesAsync();
                _logger.LogInformation("OperationModel created with ID {id} by {user}", operationModel.Id, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(operationModel);
        }

        // GET: OperationModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("OperationModelController.Edit GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var operationModel = await _context.OperationModels.FindAsync(id);
            if (operationModel == null)
            {
                return NotFound();
            }
            return View(operationModel);
        }

        // POST: OperationModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,OperationalProcess,ProcessOwner,Inputs,Outputs,KeyActivities,ResourcesRequired,PerformanceMetrics,Challenges,UserCreated,DateCreated")] 
            OperationModel operationModel)
        {
            _logger.LogInformation("OperationModelController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != operationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    operationModel.UserModified = User.Identity.Name;
                    operationModel.DateModified = DateTime.UtcNow;
                    _context.Update(operationModel);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("OperationModel updated with ID {id} by {user}", operationModel.Id, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationModelExists(operationModel.Id))
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
            return View(operationModel);
        }

        // GET: OperationModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("OperationModelController.Delete GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var operationModel = await _context.OperationModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operationModel == null)
            {
                return NotFound();
            }

            return View(operationModel);
        }

        // POST: OperationModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("OperationModelController.DeleteConfirmed POST called for id {id} by {user}", id, User.Identity.Name);
            var operationModel = await _context.OperationModels.FindAsync(id);
            _context.OperationModels.Remove(operationModel);
            await _context.SaveChangesAsync();
            _logger.LogInformation("OperationModel deleted with ID {id} by {user}", id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool OperationModelExists(int id)
        {
            return _context.OperationModels.Any(e => e.Id == id);
        }
    }
}