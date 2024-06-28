using Esercizio_M4_W2_D5.Models;

namespace Esercizio_M4_W2_D5.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();

        Product GetProductById(int id);
    }
}
