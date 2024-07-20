using System.ComponentModel.DataAnnotations;

namespace Esercizio_M5_W1_D5.Models
{
    public class Violazione
    {
        public int IDViolazione { get; set; }

        [Required]
        public string Descrizione { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Importo deve essere un valore positivo.")]
        public decimal Importo { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Decurtamento Punti deve essere un valore positivo.")]
        public int DecurtamentoPunti { get; set; }
    }
}
