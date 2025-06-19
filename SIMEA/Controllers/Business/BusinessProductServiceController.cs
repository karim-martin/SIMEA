using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class BusinessProductServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BusinessProductServiceController> _logger;

        public BusinessProductServiceController(ApplicationDbContext context, ILogger<BusinessProductServiceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: BusinessProductService
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("User {User} is accessing BusinessProductService Index at {Time}.", User.Identity.Name, DateTime.UtcNow);
            return View(await _context.BusinessProductServiceViews.ToListAsync());
        }

        // GET: BusinessProductService/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details request with null ID at {Time}.", DateTime.UtcNow);
                return NotFound();
            }

            var productService = await _context.BusinessProductServiceViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productService == null)
            {
                _logger.LogWarning("ProductService with ID {Id} not found at {Time}.", id, DateTime.UtcNow);
                return NotFound();
            }

            _logger.LogInformation("User {User} is accessing BusinessProductService Details for ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
            return View(productService);
        }

        // GET: ServiceCatalogue/Create
        public IActionResult Create()
        {
            _logger.LogInformation("User {User} is accessing BusinessProductService Create at {Time}.", User.Identity.Name, DateTime.UtcNow);
            return View();
        }

        // POST: BusinessProductService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessProductServiceView productServices)
        {
            if (ModelState.IsValid)
            {
                productServices.UserCreated = User.Identity.Name;
                productServices.DateCreated = DateTime.UtcNow;
                
                // Initialize collections if null
                productServices.KeyFeatures ??= new System.Collections.Generic.List<string>();
                productServices.Dependencies ??= new System.Collections.Generic.List<string>();
                productServices.Competitors ??= new System.Collections.Generic.List<string>();
                
                _context.Add(productServices);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User {User} created BusinessProductService with ID {Id} at {Time}.", User.Identity.Name, productServices.Id, DateTime.UtcNow);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Invalid model state in Create action at {Time}.", DateTime.UtcNow);
            return View(productServices);
        }

        // GET: BusinessProductService/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit request with null ID at {Time}.", DateTime.UtcNow);
                return NotFound();
            }

            var productService = await _context.BusinessProductServiceViews.FindAsync(id);
            if (productService == null)
            {
                _logger.LogWarning("ProductService with ID {Id} not found for editing at {Time}.", id, DateTime.UtcNow);
                return NotFound();
            }

            _logger.LogInformation("User {User} is accessing BusinessProductService Edit for ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
            return View(productService);
        }

        // POST: BusinessProductService/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ProductServiceName,Description,TargetMarketCustomer,ValueProposition,RevenueModel,KeyFeatures,Dependencies,Competitors")] 
            BusinessProductServiceView productService)
        {
            if (id != productService.Id)
            {
                _logger.LogWarning("Edit request with ID mismatch at {Time}.", DateTime.UtcNow);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productService.UserModified = User.Identity.Name;
                    productService.DateModified = DateTime.UtcNow;
                    
                    // Initialize collections if null
                    productService.KeyFeatures ??= new System.Collections.Generic.List<string>();
                    productService.Dependencies ??= new System.Collections.Generic.List<string>();
                    productService.Competitors ??= new System.Collections.Generic.List<string>();
                    
                    _context.Update(productService);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User {User} updated BusinessProductService with ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductServiceExists(productService.Id))
                    {
                        _logger.LogWarning("Concurrency exception: ProductService with ID {Id} not found at {Time}.", productService.Id, DateTime.UtcNow);
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Invalid model state in Edit action at {Time}.", DateTime.UtcNow);
            return View(productService);
        }

        // GET: BusinessProductService/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete request with null ID at {Time}.", DateTime.UtcNow);
                return NotFound();
            }

            var productService = await _context.BusinessProductServiceViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productService == null)
            {
                _logger.LogWarning("ProductService with ID {Id} not found for deletion at {Time}.", id, DateTime.UtcNow);
                return NotFound();
            }

            _logger.LogInformation("User {User} is accessing BusinessProductService Delete for ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
            return View(productService);
        }

        // POST: BusinessProductService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productService = await _context.BusinessProductServiceViews.FindAsync(id);
            _context.BusinessProductServiceViews.Remove(productService);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {User} deleted BusinessProductService with ID {Id} at {Time}.", User.Identity.Name, id, DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductServiceExists(int id)
        {
            return _context.BusinessProductServiceViews.Any(e => e.Id == id);
        }
    }
}