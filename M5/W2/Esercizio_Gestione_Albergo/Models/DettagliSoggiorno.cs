using System.ComponentModel.DataAnnotations;

namespace Esercizio_Gestione_Albergo.Models
{
    public class DettagliSoggiorno
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Descrizione { get; set; }
    }
}
