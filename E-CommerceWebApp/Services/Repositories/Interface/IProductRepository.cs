using E_CommerceWebApp.Models;

namespace E_CommerceWebApp.Services.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsWithPaginationAsync(string searchQuery, int pageNumber, int pageSize);
        Task<int> GetProductCountAsync();
        Task<Product> GetProductByIDAsync(int productId);
        Task AddNewProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
    }
}
