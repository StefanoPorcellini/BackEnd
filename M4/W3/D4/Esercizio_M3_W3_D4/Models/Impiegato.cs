using System.ComponentModel.DataAnnotations;

namespace Esercizio_M3_W3_D4.Models
{
    public class Impiegato
    {
        public int IDImpiegato { get; set; }
        [Required, Display(Name  = "Cognome")]
        public string Cognome { get; set; }
        [Required, Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required, Display(Name = "Codice Fiscale")]
        public string CodiceFiscale { get; set; }
        [Required, Display(Name = "Età")]
        public int Eta { get; set; }
        [Required, Display(Name = "Reddito Mensile")]
        public decimal RedditoMensile { get; set; }
        [Required, Display(Name = "Detrazione Fiscale")]
        public bool DetrazioneFiscale { get; set; }
    }
}
