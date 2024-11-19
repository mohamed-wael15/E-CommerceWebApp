using E_CommerceWebApp.Models;

namespace E_CommerceWebApp.Services.Repositories.Interface
{
    public interface ICategoryRepository
    {
        public Task<Category> GetCategoryByIDAsync(int categoryID);
        public Task<Category> GetCategoryByNameAsync(string categoryName);
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task<IEnumerable<Category>> GetCategoriesWithPaginationAsync(string searchQuery, int pageNumber, int pageSize);
        public Task<bool> CheckCategoryExistAsync(int id);
        public Task<Category> AddNewCategoryAsync(Category category);
        public Task<Category> UpdateCategoryAsync(Category category);
        public Task<Category> DeleteCategoryAsync(int categoryID);
    }
}
