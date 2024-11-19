using E_CommerceWebApp.Models;
using E_CommerceWebApp.Services;
using E_CommerceWebApp.Services.Repositories.Interface;
using E_CommerceWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_CommerceWebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ImageService _imageService;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(
            IProductRepository productRepository,
            ImageService imageService,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _imageService = imageService;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            int totalRecords = await _productRepository.GetProductCountAsync();
            int lastPage = (int)Math.Ceiling((double)totalRecords / pageSize);

            // checking
            if (pageNumber < 1) pageNumber = 1;
            else if (pageNumber > lastPage) pageNumber = lastPage;
            else if (pageSize > totalRecords) pageSize = totalRecords;

            var products = await _productRepository.GetProductsWithPaginationAsync(null, pageNumber, pageSize);

            return View(new PagedViewModel<IEnumerable<Product>>
                            (products, pageNumber, pageSize, totalRecords, null));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DisplayProducts(string searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            int totalRecords = await _productRepository.GetProductCountAsync();
            int lastPage = (int)Math.Ceiling((double)totalRecords / pageSize);

            // checking
            if (pageNumber < 1) pageNumber = 1;
            else if (pageNumber > lastPage) pageNumber = lastPage;
            else if (pageSize > totalRecords) pageSize = totalRecords;

            var products = await _productRepository.GetProductsWithPaginationAsync(searchQuery, pageNumber, pageSize);

            return View(new PagedViewModel<IEnumerable<Product>>
                            (products, pageNumber, pageSize, totalRecords, searchQuery));
        }

        public async Task<IActionResult> Create()
        {
            var createProductViewModel = new CreateProductViewModel();
            var categoryList = await _categoryRepository.GetAllCategoriesAsync();

            createProductViewModel.Categories = categoryList.Select(
                c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryID.ToString()
                }
                ).ToList();

            return View(createProductViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel createProductViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_imageService.CheckImage(createProductViewModel.ProductImage))
                {
                    return View(createProductViewModel);
                }

                if (!await _categoryRepository.CheckCategoryExistAsync(int.Parse(createProductViewModel.SelectedCategoryID)))
                {
                    ModelState.AddModelError("SelectedCategoryID", "The selected category does not exist.");
                    return View(createProductViewModel);
                }

                var newProduct = createProductViewModel.toProduct();

                await _productRepository.AddNewProductAsync(newProduct);


                return RedirectToAction(nameof(Index));
            }
            return View(createProductViewModel);
        }
        public async Task<IActionResult> Details(int productId)
        {
            var product = await _productRepository.GetProductByIDAsync(productId);

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int productId)
        {
            var product = await _productRepository.GetProductByIDAsync(productId);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            try
            {
                await _productRepository.UpdateProductAsync(product);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int productID)
        {
            await _productRepository.DeleteProductAsync(productID);

            return RedirectToAction(nameof(Index));
        }
    }
}
