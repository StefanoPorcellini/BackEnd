using System;
using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo.Models
{
    public class Prenotazione
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(16)]
        public string ClienteCodiceFiscale { get; set; }

        [Required]
        public int CameraNumero { get; set; }

        [Required]
        public DateTime DataPrenotazione { get; set; }

        [Required]
        public int NumeroProgressivo { get; set; }

        [Required]
        public int Anno { get; set; }

        [Required]
        public DateTime Dal { get; set; }

        [Required]
        public DateTime Al { get; set; }

        [Required]
        public decimal CaparraConfirmatoria { get; set; }

        [Required]
        public decimal Tariffa { get; set; }

        [Required]
        public int DettagliSoggiornoId { get; set; }
    }
}
