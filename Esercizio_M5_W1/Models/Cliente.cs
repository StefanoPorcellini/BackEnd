using System.ComponentModel.DataAnnotations;

namespace Esercizio_M5_W1.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string tipoCliente { get; set; }
        [StringLength(16)]
        public string CF { get; set; }
        [StringLength(11)]
        public string PIVA { get; set; }
        public int Id_User { get; set; }
    }
}
