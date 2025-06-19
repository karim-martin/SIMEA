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
    public class TechnologyTrendController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechnologyTrendController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TechnologyTrend
        public async Task<IActionResult> Index()
        {
            var trends = await _context.TechnologyTrends.ToListAsync();
            return View(trends);
        }

        // GET: TechnologyTrend/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technologyTrend = await _context.TechnologyTrends
                .FirstOrDefaultAsync(m => m.Id == id);
            if (technologyTrend == null)
            {
                return NotFound();
            }

            return View(technologyTrend);
        }

        // GET: TechnologyTrend/Create
        public IActionResult Create()
        {
            var model = new TechnologyTrendModel();
            return View(model);
        }

        // POST: TechnologyTrend/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrendName,Description,TechnologyCategory,MaturityLevel,AdoptionTimeline,StrategicRelevance,DisruptionPotential,InvestmentRequired,RiskLevel,AssessmentStatus,NextReviewDate,BusinessImpact,ExpectedBusinessValue")] TechnologyTrendModel technologyTrend)
        {
            if (ModelState.IsValid)
            {
                technologyTrend.UserCreated = User.Identity.Name ?? "System";
                technologyTrend.DateCreated = DateTime.Now;
                technologyTrend.DateModified = DateTime.Now;
                
                _context.Add(technologyTrend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(technologyTrend);
        }

        // GET: TechnologyTrend/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technologyTrend = await _context.TechnologyTrends.FindAsync(id);
            if (technologyTrend == null)
            {
                return NotFound();
            }
            return View(technologyTrend);
        }

        // POST: TechnologyTrend/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrendName,Description,TechnologyCategory,MaturityLevel,AdoptionTimeline,StrategicRelevance,DisruptionPotential,InvestmentRequired,RiskLevel,AssessmentStatus,NextReviewDate,BusinessImpact,ExpectedBusinessValue,UserCreated,DateCreated")] TechnologyTrendModel technologyTrend)
        {
            if (id != technologyTrend.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    technologyTrend.UserModified = User.Identity.Name ?? "System";
                    technologyTrend.DateModified = DateTime.Now;
                    
                    _context.Update(technologyTrend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechnologyTrendExists(technologyTrend.Id))
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
            return View(technologyTrend);
        }

        // GET: TechnologyTrend/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technologyTrend = await _context.TechnologyTrends
                .FirstOrDefaultAsync(m => m.Id == id);
            if (technologyTrend == null)
            {
                return NotFound();
            }

            return View(technologyTrend);
        }

        // POST: TechnologyTrend/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var technologyTrend = await _context.TechnologyTrends.FindAsync(id);
            if (technologyTrend != null)
            {
                _context.TechnologyTrends.Remove(technologyTrend);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechnologyTrendExists(int id)
        {
            return _context.TechnologyTrends.Any(e => e.Id == id);
        }
    }
} 