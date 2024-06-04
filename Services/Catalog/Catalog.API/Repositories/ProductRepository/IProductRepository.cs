using Catalog.API.Models;

namespace Catalog.API.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<Product> GetProductById(string id);
        Task<List<Product>> GetAllProducts();
        Task<List<Product>> GetProductByName(string Name);
        Task<List<Product>> GetProductByCategory(string Category);
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string Id);
    }
}
