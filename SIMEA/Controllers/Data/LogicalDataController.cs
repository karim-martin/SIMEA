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
    public class LogicalDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogicalDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LogicalDataModel
        public async Task<IActionResult> Index()
        {
            return View(await _context.LogicalDataModels.ToListAsync());
        }

        // GET: LogicalDataModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logicalData = await _context.LogicalDataModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logicalData == null)
            {
                return NotFound();
            }

            return View(logicalData);
        }

        // GET: LogicalDataModel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LogicalDataModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LogicalDataModel logicalDatas)
        {
            if (ModelState.IsValid)
            {
                logicalDatas.UserCreated = User.Identity.Name;
                logicalDatas.DateCreated = DateTime.UtcNow;
                _context.Add(logicalDatas);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(logicalDatas);
        }

        // GET: LogicalDataModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logicalData = await _context.LogicalDataModels.FindAsync(id);
            if (logicalData == null)
            {
                return NotFound();
            }
            return View(logicalData);
        }

        // POST: LogicalDataModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,EntityName,Attributes,Relationships,PrimaryKey,ForeignKey,Owner,Description,Dependencies,UserCreated,DateCreated")] 
            LogicalDataModel logicalData)
        {
            if (id != logicalData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    logicalData.UserModified = User.Identity.Name;
                    logicalData.DateModified = DateTime.UtcNow;
                    _context.Update(logicalData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogicalDataModelExists(logicalData.Id))
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
            return View(logicalData);
        }

        // GET: LogicalDataModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logicalData = await _context.LogicalDataModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logicalData == null)
            {
                return NotFound();
            }

            return View(logicalData);
        }

        // POST: LogicalDataModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logicalData = await _context.LogicalDataModels.FindAsync(id);
            _context.LogicalDataModels.Remove(logicalData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogicalDataModelExists(int id)
        {
            return _context.LogicalDataModels.Any(e => e.Id == id);
        }
    }
}