using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Esercizio_Pizzeria_In_Forno.Models
{
    public class ProductToOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public int IdOrder { get; set; }
        public int Quantity { get; set; }
    }
}