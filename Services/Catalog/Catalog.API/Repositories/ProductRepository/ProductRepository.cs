using Catalog.API.Data;
using Catalog.API.Models;
using MongoDB.Driver;

namespace Catalog.API.Repositories.ProductRepository
{
    public class ProductRepository(ICatalogContext catalogContext) : IProductRepository
    {
        private readonly ICatalogContext _Context = catalogContext;
       

        public async Task<List<Product>> GetAllProducts()
        {
            return await _Context
                .Products
                .Find(p => true)
                .ToListAsync();
        }


        public async Task<Product> GetProductById(string id)
        {
            return await _Context
                .Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductByCategory(string Category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, Category);
            return await _Context
               .Products
               .Find(filter)
               .ToListAsync();
        }

        public async Task<List<Product>> GetProductByName(string Name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, Name);
            return await _Context
               .Products
               .Find(filter)
               .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
             await _Context
                .Products
                .InsertOneAsync(product);
        }

      
        public async Task<bool> UpdateProduct(Product product)
        {
           var  Result = await _Context
                .Products
                .ReplaceOneAsync(filter : p => p.Id == product.Id , replacement: product);
            return (Result.IsAcknowledged && Result.ModifiedCount > 0);
        }

        public async Task<bool> DeleteProduct(string Id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, Id);
            var Result = await _Context
                .Products
                .DeleteOneAsync(filter);
            return (Result.IsAcknowledged && Result.DeletedCount > 0);
        }
    }
}
