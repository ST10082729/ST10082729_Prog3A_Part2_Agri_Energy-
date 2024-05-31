using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriEnergy.Models;

namespace AgriEnergy.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AgriDB _context;

        public ProductsController(AgriDB context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var agriDB = _context.Products.Include(p => p.Farmer);
            var productList = await agriDB.ToListAsync();

            // Log the product list to the console
            Console.WriteLine("Product List:");
            foreach (var product in productList)
            {
                Console.WriteLine($"ProductId: {product.ProductId}, Name: {product.Name}, FarmerId: {product.FarmerId}");
            }

            return View(productList);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Category,ProductionDate,FarmerId")] Product product)
        {
            Console.WriteLine("Entering Create Action");
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("Model State is Valid");
                    Console.WriteLine($"Product Details: Id={product.ProductId}, Name={product.Name}, Category={product.Category}, ProductionDate={product.ProductionDate}, FarmerId={product.FarmerId}");
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Product created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating product: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
                }
            }
            else
            {
                Console.WriteLine("Model State is Invalid");
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                    }
                }
            }

            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.FarmerId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.FarmerId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Category,ProductionDate,FarmerId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.FarmerId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'AgriDB.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}