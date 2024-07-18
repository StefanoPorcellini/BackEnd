namespace Esercizio_M5_W1.Models
{
    public class Spedizione
    {
        public int Id { get; set; }
        public int Id_Cliente { get; set; }
        public Cliente Cliente { get; set; }
        public string N_Identificativo { get; set; }
        public DateTime DataSpedizione { get; set; }
        public double Peso { get; set; }
        public string CittaDestinaratia { get; set; }
        public string IndirizzoDestinazione { get; set; }
        public string NominativoDestinazione { get; set; }
        public decimal Costo {  get; set; }
        public DateTime DataConsegnaPrevista { get; set; }
        public ICollection<AggiornamentoSpedizione> Aggiornamenti { get; set; }
    }
}
