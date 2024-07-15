namespace Esercizio_M5_W1.Models
{
    public class AggiornamentoSpedizione
    {
        public int Id { get; set; }
        public int Id_Spedizione { get; set; }
        public Spedizione Spedizione { get; set; }
        public string Stato { get; set; }
        public string Luogo { get; set; }
        public string Descrizione { get; set; }
        public DateTime Data_Ora { get; set; }
    }
}