using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esercizio_Pizzeria_In_Forno.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public required string Name { get; set; }
        [Required, StringLength(128)]
        public required string Photo { get; set; }
        [Required, Precision(18, 2)]
        public required decimal Price { get; set; }
        [Range(0, 60)]
        public int DeliveryTimeInMinutes { get; set; }
        public virtual List<Ingredient> Ingredients { get; set; } = [];

    }
}
