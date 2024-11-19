using E_CommerceWebApp.Data;
using E_CommerceWebApp.Models;
using E_CommerceWebApp.Services.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
            => await _context.Products.Include(p => p.ProductImage).ToListAsync();

        public async Task<IEnumerable<Product>> GetProductsWithPaginationAsync(string searchQuery, int pageNumber, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.ProductName.Contains(searchQuery));
            }

            int startingIndex = (pageNumber - 1) * pageSize;

            if (startingIndex < 0)
                return new List<Product>();

            return await query
                        .Include(p => p.ProductImage)
                        .OrderBy(p => p.ProductId)
                        .Skip(startingIndex)
                        .Take(pageSize)
                        .ToListAsync();
        }

        public async Task<int> GetProductCountAsync()
            => await _context.Products.CountAsync();

        public async Task<Product> GetProductByIDAsync(int productId)
            => await _context.Products
                     .Include(p => p.Category)
                     .FirstOrDefaultAsync(p => p.ProductId == productId);

        public async Task AddNewProductAsync(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (product.CategoryId <= 0 || !await _context.Categories.AnyAsync(c => c.CategoryID == product.CategoryId))
            {
                throw new ArgumentException("Invalid CategoryId.");
            }
            Console.WriteLine($"Update ProductId {product.ProductId} With CategoryId {product.CategoryId}");

            _context.Products.Update(product);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var existingProduct = await GetProductByIDAsync(productId);

            if (existingProduct != null)
            {
                _context.Products.Remove(existingProduct);

                await _context.SaveChangesAsync();
            }
        }
    }
}
