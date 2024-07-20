namespace Esercizio_M5_W1_D5.Models
{
    //estensione del verbale per la visualizzazione dei dati riferiti alle chiavi esterne 
    public class VerbaleViewModel : Verbale
    {
        public Anagrafica Trasgressore { get; set; }
        public Violazione Violazione { get; set; }

    }
}
