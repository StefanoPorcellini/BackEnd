using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo.Models
{
    public class ServizioAggiuntivo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Descrizione { get; set; }
        [Required]
        public decimal Prezzo { get; set; }
    }
}
