namespace Esercizio_M5_W1.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TipoCliente tipoCliente { get; set; }
        public string CF { get; set; }
        public string PIVA { get; set; }
    }
}
