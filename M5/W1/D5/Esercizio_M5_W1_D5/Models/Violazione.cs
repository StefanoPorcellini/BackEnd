namespace Esercizio_M5_W1_D5.Models
{
    public class Violazione
    {
        public int IDViolazione { get; set; }
        public string Descrizione { get; set; }
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
    }
}
