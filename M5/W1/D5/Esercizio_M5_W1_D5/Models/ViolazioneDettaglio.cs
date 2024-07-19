namespace Esercizio_M5_W1_D5.Models
{
    public class ViolazioneDettaglio
    {
        public string Descrizione { get; set; }
        public decimal Importo { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public DateTime DataViolazione { get; set; }
        public int DecurtamentoPunti { get; set; }

    }
}
