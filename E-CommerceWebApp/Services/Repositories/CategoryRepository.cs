using E_CommerceWebApp.Data;
using E_CommerceWebApp.Models;
using E_CommerceWebApp.Services.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
             _context = context;
        }
        public async Task<Category> GetCategoryByIDAsync(int categoryID)
        {
            var category = await _context.Categories
                                 .Include(c => c.Products)
                                 .FirstOrDefaultAsync(c => c.CategoryID == categoryID);

            if (category != null && category.Products == null)
            {
                category.Products = new List<Product>(); 
            }

            return category;
        }
           

        public async Task<Category> GetCategoryByNameAsync(string categoryName)
            => await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
            =>  await _context.Categories.ToListAsync();
        
        public Task<IEnumerable<Category>> GetCategoriesWithPaginationAsync(string searchQuery, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> CheckCategoryExistAsync(int id)
            => await _context.Categories.AnyAsync(c => c.CategoryID == id);

        public async Task<Category> AddNewCategoryAsync(Category newCategory)
        {
            if (newCategory == null || string.IsNullOrEmpty(newCategory.CategoryName))
                throw new ArgumentException("Invalid category data.");

            if (newCategory.CategoryID != 0)
                throw new ArgumentException("Invalid category ID.");

            var createdCategory = await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return createdCategory.Entity;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            if (category == null || string.IsNullOrEmpty(category.CategoryName))
                throw new ArgumentException("Invalid category data.");

            if (category.CategoryID <= 0)
                throw new ArgumentException("Invalid category ID.");

            if (!await CheckCategoryExistAsync(category.CategoryID))
                return null;

            var updatedCategory = _context.Categories.Update(category);

            _context.SaveChanges();

            return updatedCategory.Entity;
        }

        public async Task<Category> DeleteCategoryAsync(int categoryID)
        {
            if (categoryID <= 0)
                throw new ArgumentException("Invalid category ID.");

            var category = await GetCategoryByIDAsync(categoryID);

            if (category is null) return null;

            var deletedCategory = _context.Categories.Remove(category);

            _context.SaveChanges();

            return deletedCategory.Entity;
        }
    }
}
