namespace Esercizio_M5_W1_D5.Models
{
    public class Verbale
    {
        public int IDVerbale { get; set; }
        public DateTime DataViolazione { get; set; }
        public string IndirizzoViolazione { get; set; }
        public string NominativoAgente { get; set; }
        public DateTime DataTrascrizioneVerbale { get; set; }
        public int Anagrafica_FK { get; set; }
        public int Violazione_FK { get; set; }
        public Anagrafica Trasgressore { get; set; }
        public Violazione Violazione {get; set; }

    }
}
