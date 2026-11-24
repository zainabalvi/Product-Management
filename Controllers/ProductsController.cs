
using Microsoft.AspNetCore.Mvc;
using pms_mvc_example.Data;
using pms_mvc_example.Models;

namespace pms_mvc_example.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductRepository _repository;

        public ProductsController(ProductRepository repository)
        {
            _repository = repository;
        }

        // Display all products
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAllAsync();
            return View(products);
        }

        // Display the Create form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Save a new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            await _repository.CreateAsync(product);

            TempData["Success"] = "Product created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Display the Edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // Save edited product information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(product);

            var existingProduct = await _repository.GetByIdAsync(id);

            if (existingProduct == null)
                return NotFound();

            await _repository.UpdateAsync(product);

            TempData["Success"] = "Product updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Display Delete confirmation
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // Delete the product after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            await _repository.DeleteAsync(id);

            TempData["Success"] = "Product deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
