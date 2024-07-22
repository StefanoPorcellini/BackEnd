using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo
{ 
         public class Cliente
    {
        [Key]
        [StringLength(16)]
        public string CodiceFiscale { get; set; }

        [Required]
        [StringLength(50)]
        public string Cognome { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Città { get; set; }

        [Required]
        [StringLength(2)]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "La provincia deve contenere esattamente due caratteri maiuscoli.")]
        public string Provincia { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(20)]
        public string Cellulare { get; set; }
    }
}