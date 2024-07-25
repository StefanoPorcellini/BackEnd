using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string NumeroProgressivo { get; set; }

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
        public decimal SaldoFinale { get; set; }

        public int DettagliSoggiornoId { get; set; }
    }
}
