using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esercizio_Pizzeria_In_Forno.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Address { get; set; }
        [StringLength(200)]
        public string Note { get; set; }
        public bool Processed { get; set; } = false;
        public List<User> Users { get; set; } = [];
        public List<ProductToOrder> Products { get; set; } = [];

    }
}
