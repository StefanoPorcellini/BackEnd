using System.ComponentModel.DataAnnotations;

namespace Esercizio_M3_W3_D4.Models
{
    public class Impiego
    {
        public int IDImpiego {  get; set; }
        [Required, Display(Name = "Tipo Impiego")]
        public string TipoImpiego { get; set; }
        [Required, Display(Name = "Data Assunzione")]
        public DateTime Assunzione { get; set; }
        public int IDImpiegato { get; set; }
    }
}
