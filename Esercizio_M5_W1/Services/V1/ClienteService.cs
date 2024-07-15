using Esercizio_M5_W1.Models;
using System.Data.Common;

namespace Esercizio_M5_W1.Services.V1
{
    public class ClienteService : SqlServerServiceBase, IClienteService
    {
        //Lista clienti
        public static List<Cliente> _cliente = new List<Cliente>();

        public ClienteService(IConfiguration config) : base(config)
        {
        }

        //Creazione Clienti
        public Cliente Create(DbDataReader reader)
        {
            return new Cliente
            {
                Nome = reader.GetString(0),
                tipoCliente = reader.GetString(1),
                CF = reader.GetString(2),
                PIVA = reader.GetString(3)
            };
        }

    }
}
