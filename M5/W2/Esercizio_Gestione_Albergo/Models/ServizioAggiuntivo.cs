using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo.Models
{
    public class ServizioAggiuntivo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int PrenotazioneID { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public int Quantità { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Prezzo { get; set; }

        [Required]
        [StringLength(255)]
        public string Descrizione { get; set; }

        [ForeignKey("PrenotazioneID")]
        public Prenotazione Prenotazione { get; set; }

    }
}
