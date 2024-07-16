using Esercizio_M5_W1.Models;
using System.Data.Common;
using System.Data.SqlClient;

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

        public void Create(Cliente cliente)
        {
            var query = "INSERT INTO Clienti (Nome, Tipo, CodiceFiscale, PartitaIva) " +
                        "VALUES (@Nome, @tipoCliente, @CF, @PIVA) SELECT SCOPE_IDENTITY()";
            var cmd = GetCommand(query);
            cmd.Parameters.Add(new SqlParameter("@Nome",  cliente.Nome));
            cmd.Parameters.Add(new SqlParameter("@tipoCliente", cliente.tipoCliente));
            cmd.Parameters.Add(new SqlParameter("@CF", cliente.CF));
            cmd.Parameters.Add(new SqlParameter("@PIVA", cliente.PIVA));

            using var conn = GetConnection();
            conn.Open();
            var result  = cmd.ExecuteScalar();
            if (result == null)
                throw new Exception("Creazione Cliente non completata");
            cliente.Id = Convert.ToInt32(result);
        }

        public void Delete(int Id)
        {
            var query = "DELETE FROM Clienti WHERE Id = @Id";
            var cmd = GetCommand(query);
            cmd.Parameters.Add(new SqlParameter("Id", Id));
            using var conn = GetConnection();
            conn.Open();
            var result = cmd.ExecuteNonQuery();
            if (result != 1) throw new Exception("Cliente non eliminato");
        }

        public IEnumerable<Cliente> GetAll()
        {
            var query = "SELECT Nome, Tipo, CodiceFiscale, PartitaIva FROM Clienti";
            var cmd = GetCommand(query);
            using var conn = GetConnection();
            conn.Open();
            var reader = cmd.ExecuteReader();
            var listaClienti = new List<Cliente>();
            while (reader.Read()) listaClienti.Add(Create(reader));
            return listaClienti;
        }

        public Cliente GetById(int Id)
        {
            var query = "SELECT Nome, Tipo, CodiceFiscale, PartitaIva FROM Clienti " +
                "WHERE Id = @Id";
            var cmd = GetCommand(query);
            cmd.Parameters.Add(new SqlParameter("Id", Id));
            using var conn = GetConnection();
            conn.Open();
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Cliente cliente = new Cliente()
                {
                    Nome = reader.GetString(0),
                    tipoCliente = reader.GetString(1),
                    CF = reader.GetString(2),
                    PIVA = reader.GetString(3)
                };
                return cliente;
            } else
                {
                throw new Exception("Cliente non trovato");
                }
        }
        public void Update(Cliente cliente)
        {
            throw new NotImplementedException();
        }
    }
}
