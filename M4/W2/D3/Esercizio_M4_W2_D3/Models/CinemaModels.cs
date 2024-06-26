using System.ComponentModel.DataAnnotations;

namespace Esercizio_M4_W2_D3.Models
{
    public enum TipoBiglietto
    {
        Intero,
        Ridotto
    }

    public class Ospite
    {
        [Display(Name = "Nome: ")]
        public string Nome { get; set; }
        [Display(Name = "Cognome: ")]
        public string Cognome { get; set; }
        [Display(Name = "Sala: ")]
        public string Sala { get; set; }
        [Display(Name = "Tipo di Biglietto: ")]
        public TipoBiglietto TipoBiglietto { get; set; }
    }

    public class Sala
    {
        public string Nome { get; set; }
        public int CapienzaMassima { get; set; } = 120;
        public int BigliettiVenduti { get; set; }
        public int BigliettiRidotti { get; set; }

        public Sala(string nome)
        {
            Nome = nome;
            BigliettiVenduti = 0;
            BigliettiRidotti = 0;
        }
    }

    public static class CinemaData
    {
        public static List<Sala> Sale = new List<Sala>
        {
            new Sala("SALA NORD"),
            new Sala("SALA EST"),
            new Sala("SALA SUD")
        };

        public static List<Ospite> Ospiti = new List<Ospite>();

    }

}
