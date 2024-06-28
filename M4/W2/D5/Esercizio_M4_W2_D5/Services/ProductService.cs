using Esercizio_M4_W2_D5.Models;

namespace Esercizio_M4_W2_D5.Services
{
    public class ProductService : IProductService
    {
        private static List<Product> _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Brand = "Nike",
                Name = "Air Zoom",
                Price = 129.99m,
                Description = "Scarpe da tennis leggere e reattive con tecnologia Air Zoom.",
                ImgCover = "nike_air_zoom.jpg",
                OtherImg = new List<string> { "nike_air_zoom_2.jpg", "nike_air_zoom_3.jpg" }
            },
            new Product
            {
                Id = 2,
                Brand = "Adidas",
                Name = "Ultraboost",
                Price = 149.99m,
                Description = "Scarpe da tennis con ammortizzazione Boost per massima energia.",
                ImgCover = "adidas_ultraboost.jpg",
                OtherImg = new List<string> { "adidas_ultraboost_2.jpg", "adidas_ultraboost_3.jpg" }
            }
        };

        public List<Product> GetProducts()
        {
            return _products;
        }

        public Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }
}
