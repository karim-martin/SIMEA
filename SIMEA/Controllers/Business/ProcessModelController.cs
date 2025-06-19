using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class ProcessModelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProcessModelController> _logger;

        public ProcessModelController(ApplicationDbContext context, ILogger<ProcessModelController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ProcessModel
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ProcessModelController.Index called by {user}", User.Identity.Name);
            return View(await _context.ProcessModels.ToListAsync());
        }

        // GET: ProcessModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("ProcessModelController.Details called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var processModel = await _context.ProcessModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processModel == null)
            {
                return NotFound();
            }

            return View(processModel);
        }

        // GET: ProcessModel/Create
        public IActionResult Create()
        {
            _logger.LogInformation("ProcessModelController.Create GET called by {user}", User.Identity.Name);
            return View();
        }

        // POST: ProcessModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProcessModel processModel)
        {
            _logger.LogInformation("ProcessModelController.Create POST called by {user}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                // Process TimeSpan values for ProcessStages
                if (processModel.Stages != null && processModel.Stages.Any())
                {
                    foreach (var stage in processModel.Stages)
                    {
                        // Ensure process time and wait time are properly parsed from the form
                        if (stage.ProcessTime == TimeSpan.Zero && Request.Form.ContainsKey($"Stages[{processModel.Stages.IndexOf(stage)}].ProcessTime"))
                        {
                            var processTimeStr = Request.Form[$"Stages[{processModel.Stages.IndexOf(stage)}].ProcessTime"].ToString();
                            if (TimeSpan.TryParse(processTimeStr, out TimeSpan processTime))
                            {
                                stage.ProcessTime = processTime;
                            }
                        }

                        if (stage.WaitTime == TimeSpan.Zero && Request.Form.ContainsKey($"Stages[{processModel.Stages.IndexOf(stage)}].WaitTime"))
                        {
                            var waitTimeStr = Request.Form[$"Stages[{processModel.Stages.IndexOf(stage)}].WaitTime"].ToString();
                            if (TimeSpan.TryParse(waitTimeStr, out TimeSpan waitTime))
                            {
                                stage.WaitTime = waitTime;
                            }
                        }

                        // Initialize any null collections
                        stage.ProcessIds ??= new List<string>();
                        stage.ApplicationIds ??= new List<string>();
                        stage.Issues ??= new List<string>();
                    }
                }

                processModel.UserCreated = User.Identity.Name;
                processModel.DateCreated = DateTime.UtcNow;
                _context.Add(processModel);
                await _context.SaveChangesAsync();
                _logger.LogInformation("ProcessModel created with ID {id} by {user}", processModel.Id, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(processModel);
        }

        // GET: ProcessModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("ProcessModelController.Edit GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var processModel = await _context.ProcessModels.FindAsync(id);
            if (processModel == null)
            {
                return NotFound();
            }
            return View(processModel);
        }

        // POST: ProcessModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ProcessName,ProcessDescription,Owner,Inputs,Outputs,StepsWorkflow,ToolsSystemsUsed,PerformanceMetrics,CustomerType,MappingDate,CycleEfficiency,LeadTime,KeyMetrics,Bottlenecks,ImprovementOpportunities")] 
            ProcessModel processModel)
        {
            _logger.LogInformation("ProcessModelController.Edit POST called for id {id} by {user}", id, User.Identity.Name);
            if (id != processModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Process stages from form
                    var stageKeys = Request.Form.Keys.Where(k => k.StartsWith("Stages[")).ToList();
                    if (stageKeys.Any())
                    {
                        processModel.Stages = new List<ProcessStage>();
                        var stageIndices = stageKeys
                            .Select(k => k.Split('[', ']'))
                            .Where(parts => parts.Length > 1 && int.TryParse(parts[1], out _))
                            .Select(parts => int.Parse(parts[1]))
                            .Distinct()
                            .ToList();

                        foreach (var index in stageIndices)
                        {
                            var stage = new ProcessStage
                            {
                                Id = Request.Form[$"Stages[{index}].Id"].ToString(),
                                Name = Request.Form[$"Stages[{index}].Name"].ToString(),
                                Description = Request.Form[$"Stages[{index}].Description"].ToString(),
                                IsValueAdd = Request.Form[$"Stages[{index}].IsValueAdd"].ToString() == "true",
                                ProcessIds = new List<string>(),
                                ApplicationIds = new List<string>(),
                                Issues = new List<string>()
                            };

                            if (TimeSpan.TryParse(Request.Form[$"Stages[{index}].ProcessTime"].ToString(), out TimeSpan processTime))
                            {
                                stage.ProcessTime = processTime;
                            }

                            if (TimeSpan.TryParse(Request.Form[$"Stages[{index}].WaitTime"].ToString(), out TimeSpan waitTime))
                            {
                                stage.WaitTime = waitTime;
                            }

                            processModel.Stages.Add(stage);
                        }
                    }
                    else
                    {
                        // Retrieve the existing entity to preserve stages and stakeholder values
                        var existingModel = await _context.ProcessModels
                            .Include(p => p.Stages)
                            .Include(p => p.StakeholderValues)
                            .FirstOrDefaultAsync(p => p.Id == id);

                        if (existingModel != null)
                        {
                            // Keep the existing collections
                            processModel.Stages = existingModel.Stages;
                            processModel.StakeholderValues = existingModel.StakeholderValues;
                        }
                    }

                    processModel.UserModified = User.Identity.Name;
                    processModel.DateModified = DateTime.UtcNow;
                    _context.Update(processModel);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("ProcessModel updated with ID {id} by {user}", processModel.Id, User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessModelExists(processModel.Id))
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
            return View(processModel);
        }

        // GET: ProcessModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("ProcessModelController.Delete GET called for id {id} by {user}", id, User.Identity.Name);
            if (id == null)
            {
                return NotFound();
            }

            var processModel = await _context.ProcessModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processModel == null)
            {
                return NotFound();
            }

            return View(processModel);
        }

        // POST: ProcessModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("ProcessModelController.DeleteConfirmed POST called for id {id} by {user}", id, User.Identity.Name);
            var processModel = await _context.ProcessModels.FindAsync(id);
            _context.ProcessModels.Remove(processModel);
            await _context.SaveChangesAsync();
            _logger.LogInformation("ProcessModel deleted with ID {id} by {user}", id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessModelExists(int id)
        {
            return _context.ProcessModels.Any(e => e.Id == id);
        }
    }
}