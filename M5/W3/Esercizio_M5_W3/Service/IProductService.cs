using Esercizio_Pizzeria_In_Forno.Models;

namespace Esercizio_Pizzeria_In_Forno.Service
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);

    }
}
