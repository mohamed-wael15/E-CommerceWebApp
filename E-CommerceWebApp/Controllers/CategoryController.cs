using E_CommerceWebApp.Models;
using E_CommerceWebApp.Services.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<ActionResult> Index()
        {
            var data = await _categoryRepository.GetAllCategoriesAsync();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            try
            {
                if (await _categoryRepository.GetCategoryByNameAsync(category.CategoryName) != null)
                    return View(category);

                await _categoryRepository.AddNewCategoryAsync(category);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      

        public async Task<ActionResult> Update(int id)
        {
            var product = await _categoryRepository.GetCategoryByIDAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, Category category)
        {
            try
            {
                await _categoryRepository.UpdateCategoryAsync(category);
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
