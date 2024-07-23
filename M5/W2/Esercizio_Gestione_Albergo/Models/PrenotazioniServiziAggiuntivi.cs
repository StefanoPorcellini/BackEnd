using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo.Models
{
    public class PrenotazioniServiziAggiuntivi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PrenotazioneID { get; set; }

        [Required]
        public int ServizioAggiuntivoID { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public int Quantità { get; set; }

        [Required]
        public decimal Prezzo { get; set; }
    }
}
