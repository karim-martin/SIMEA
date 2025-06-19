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
    public class InvestmentDecisionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvestmentDecisionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InvestmentDecision
        public async Task<IActionResult> Index()
        {
            var decisions = await _context.InvestmentDecisions.ToListAsync();
            return View(decisions);
        }

        // GET: InvestmentDecision/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investmentDecision = await _context.InvestmentDecisions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investmentDecision == null)
            {
                return NotFound();
            }

            return View(investmentDecision);
        }

        // GET: InvestmentDecision/Create
        public IActionResult Create()
        {
            var model = new InvestmentDecisionModel();
            return View(model);
        }

        // POST: InvestmentDecision/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvestmentName,Description,InvestmentType,InvestmentCategory,BusinessDriver,TotalInvestment,ExpectedBenefits,PaybackPeriodMonths,ROIPercentage,NPV,IRRPercentage,ImplementationTimeline,DecisionStatus,DecisionDate,DecisionMaker,DecisionRationale,PriorityScore")] InvestmentDecisionModel investmentDecision)
        {
            if (ModelState.IsValid)
            {
                investmentDecision.UserCreated = User.Identity.Name ?? "System";
                investmentDecision.DateCreated = DateTime.Now;
                investmentDecision.DateModified = DateTime.Now;
                
                _context.Add(investmentDecision);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(investmentDecision);
        }

        // GET: InvestmentDecision/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investmentDecision = await _context.InvestmentDecisions.FindAsync(id);
            if (investmentDecision == null)
            {
                return NotFound();
            }
            return View(investmentDecision);
        }

        // POST: InvestmentDecision/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvestmentName,Description,InvestmentType,InvestmentCategory,BusinessDriver,TotalInvestment,ExpectedBenefits,PaybackPeriodMonths,ROIPercentage,NPV,IRRPercentage,ImplementationTimeline,DecisionStatus,DecisionDate,DecisionMaker,DecisionRationale,PriorityScore,UserCreated,DateCreated")] InvestmentDecisionModel investmentDecision)
        {
            if (id != investmentDecision.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    investmentDecision.UserModified = User.Identity.Name ?? "System";
                    investmentDecision.DateModified = DateTime.Now;
                    
                    _context.Update(investmentDecision);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestmentDecisionExists(investmentDecision.Id))
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
            return View(investmentDecision);
        }

        // GET: InvestmentDecision/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investmentDecision = await _context.InvestmentDecisions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investmentDecision == null)
            {
                return NotFound();
            }

            return View(investmentDecision);
        }

        // POST: InvestmentDecision/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var investmentDecision = await _context.InvestmentDecisions.FindAsync(id);
            if (investmentDecision != null)
            {
                _context.InvestmentDecisions.Remove(investmentDecision);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvestmentDecisionExists(int id)
        {
            return _context.InvestmentDecisions.Any(e => e.Id == id);
        }
    }
} 