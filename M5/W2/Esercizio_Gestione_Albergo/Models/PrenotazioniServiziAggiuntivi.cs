using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo.Models
{
    public class PrenotazioniServiziAggiuntivi
    {
        
        [Key]
        public int PrenotazioneID { get; set; }

        [Key]
        public int ServizioAggiuntivoID { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public int Quantità { get; set; }
    }
}
