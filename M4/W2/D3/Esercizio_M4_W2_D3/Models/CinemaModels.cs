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
        public string Locandina { get; set; }
        public string Titolo { get; set; }

        public Sala(string nome, string locandina, string titolo)
        {
            Nome = nome;
            Locandina = locandina;
            Titolo = titolo;
            BigliettiVenduti = 0;
            BigliettiRidotti = 0;
        }
        public int PostiRimasti => CapienzaMassima - BigliettiVenduti;

    }

    public static class CinemaData
    {
        public static List<Sala> Sale = new List<Sala>
        {
            new Sala("SALA NORD", "https://www.ucicinemas.it/media/movie/o/2024/inside-out-2.jpg", "Inside Out 2"),
            new Sala("SALA EST", "https://pad.mymovies.it/filmclub/2020/01/056/locandina.jpg", "Bad Boys: Ride or Die"),
            new Sala("SALA SUD", "https://pad.mymovies.it/filmclub/2023/01/025/locandina.jpg", "The Bikeriders")
        };

        public static List<Ospite> Ospiti = new List<Ospite>();

    }

}
