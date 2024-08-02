using static System.Collections.Specialized.BitVector32;

namespace Esercizio_Pizzeria_In_Forno.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public List<ProductDetailsViewModel> Products { get; set; }
        public decimal TotalPrice { get; set; }

    }
}