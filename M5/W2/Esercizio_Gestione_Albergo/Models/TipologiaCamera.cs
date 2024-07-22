using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo.Models
{
    public class TipologiaCamera
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Descrizione { get; set; }
    }
}
