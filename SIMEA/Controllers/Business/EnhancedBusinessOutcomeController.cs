using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SIMEA.Controllers.Business
{
    [Authorize]
    public class EnhancedBusinessOutcomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnhancedBusinessOutcomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EnhancedBusinessOutcome
        public async Task<IActionResult> Index()
        {
            var outcomes = await _context.EnhancedBusinessOutcomes.ToListAsync();
            return View(outcomes);
        }

        // GET: EnhancedBusinessOutcome/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessOutcome = await _context.EnhancedBusinessOutcomes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessOutcome == null)
            {
                return NotFound();
            }

            return View(businessOutcome);
        }

        // GET: EnhancedBusinessOutcome/Create
        public IActionResult Create()
        {
            var model = new EnhancedBusinessOutcomeModel();
            return View(model);
        }

        // POST: EnhancedBusinessOutcome/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OutcomeName,Description,OutcomeType,OutcomeCategory,BusinessPriority,ExpectedValue,RealizationTimeline,CurrentStatus,ProgressPercentage,Owner,Sponsor,ReviewDate")] EnhancedBusinessOutcomeModel businessOutcome)
        {
            if (ModelState.IsValid)
            {
                businessOutcome.UserCreated = User.Identity.Name ?? "System";
                businessOutcome.DateCreated = DateTime.Now;
                businessOutcome.DateModified = DateTime.Now;
                
                _context.Add(businessOutcome);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(businessOutcome);
        }

        // GET: EnhancedBusinessOutcome/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessOutcome = await _context.EnhancedBusinessOutcomes.FindAsync(id);
            if (businessOutcome == null)
            {
                return NotFound();
            }
            return View(businessOutcome);
        }

        // POST: EnhancedBusinessOutcome/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OutcomeName,Description,OutcomeType,OutcomeCategory,BusinessPriority,ExpectedValue,RealizationTimeline,CurrentStatus,ProgressPercentage,Owner,Sponsor,ReviewDate,UserCreated,DateCreated")] EnhancedBusinessOutcomeModel businessOutcome)
        {
            if (id != businessOutcome.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    businessOutcome.UserModified = User.Identity.Name ?? "System";
                    businessOutcome.DateModified = DateTime.Now;
                    
                    _context.Update(businessOutcome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnhancedBusinessOutcomeExists(businessOutcome.Id))
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
            return View(businessOutcome);
        }

        // GET: EnhancedBusinessOutcome/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessOutcome = await _context.EnhancedBusinessOutcomes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessOutcome == null)
            {
                return NotFound();
            }

            return View(businessOutcome);
        }

        // POST: EnhancedBusinessOutcome/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessOutcome = await _context.EnhancedBusinessOutcomes.FindAsync(id);
            if (businessOutcome != null)
            {
                _context.EnhancedBusinessOutcomes.Remove(businessOutcome);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnhancedBusinessOutcomeExists(int id)
        {
            return _context.EnhancedBusinessOutcomes.Any(e => e.Id == id);
        }
    }
} 