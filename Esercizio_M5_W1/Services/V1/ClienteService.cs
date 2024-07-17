using Esercizio_M5_W1.Models;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Esercizio_M5_W1.Services.V1
{
    public class ClienteService : SqlServerServiceBase, IClienteService
    {
        public ClienteService(IConfiguration config) : base(config) { }

        // Creazione Cliente dal DataReader
        public Cliente Create(DbDataReader reader)
        {
            return new Cliente
            {
                Nome = reader.GetString(0),
                tipoCliente = reader.GetString(1),
                CF = reader.IsDBNull(2) ? null : reader.GetString(2),
                PIVA = reader.IsDBNull(3) ? null : reader.GetString(3),
                Id = reader.GetInt32(4)
            };
        }

        // Inserimento Cliente nel database
        public void Create(Cliente cliente)
        {
            var query = "INSERT INTO Clienti (Nome, Tipo, CodiceFiscale, PartitaIva) " +
                        "VALUES (@Nome, @tipoCliente, @CF, @PIVA); SELECT SCOPE_IDENTITY()";
            using var conn = GetConnection();
            conn.Open();
            using var cmd = GetCommand(query, conn);
            cmd.Parameters.Add(new SqlParameter("@Nome", cliente.Nome));
            cmd.Parameters.Add(new SqlParameter("@tipoCliente", cliente.tipoCliente));
            cmd.Parameters.Add(new SqlParameter("@CF", cliente.CF ?? (object)DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PIVA", cliente.PIVA ?? (object)DBNull.Value));

            var result = cmd.ExecuteScalar();
            if (result == null)
                throw new Exception("Creazione Cliente non completata");
            cliente.Id = Convert.ToInt32(result);
        }

        // Eliminazione Cliente dal database
        public void Delete(int Id)
        {
            var query = "DELETE FROM Clienti WHERE Id = @Id";
            using var conn = GetConnection();
            conn.Open();
            using var cmd = GetCommand(query, conn);
            cmd.Parameters.Add(new SqlParameter("@Id", Id));

            var result = cmd.ExecuteNonQuery();
            if (result != 1)
                throw new Exception("Cliente non eliminato");
        }

        // Recupero di tutti i clienti
        public IEnumerable<Cliente> GetAll()
        {
            var query = "SELECT Nome, Tipo, CodiceFiscale, PartitaIva, Id FROM Clienti";
            using var conn = GetConnection();
            conn.Open();
            using var cmd = GetCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            var listaClienti = new List<Cliente>();
            while (reader.Read())
            {
                listaClienti.Add(Create(reader));
            }
            return listaClienti;
        }

        // Recupero Cliente per ID
        public Cliente GetById(int Id)
        {
            var query = "SELECT Nome, Tipo, CodiceFiscale, PartitaIva, Id FROM Clienti WHERE Id = @Id";
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Create(reader);
                        }
                        else
                        {
                            throw new Exception("Cliente non trovato");
                        }
                    }
                }
            }
        }


        // Aggiornamento Cliente (non implementato)
        public void Update(Cliente cliente)
        {
            if (cliente.Id <= 0)
            {
                throw new ArgumentException("ID cliente non valido");
            }

            if (string.IsNullOrEmpty(cliente.Nome))
            {
                throw new ArgumentException("Il nome del cliente è richiesto");
            }

            var query = "UPDATE Clienti SET Nome = @Nome, Tipo = @tipoCliente, CodiceFiscale = @CF, PartitaIva = @PIVA " +
                        "WHERE Id = @Id";

            using var conn = GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction(); // Avvia una transazione

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add(new SqlParameter("@Nome", cliente.Nome));
                cmd.Parameters.Add(new SqlParameter("@tipoCliente", cliente.tipoCliente));
                cmd.Parameters.Add(new SqlParameter("@CF", string.IsNullOrEmpty(cliente.CF) ? (object)DBNull.Value : cliente.CF));
                cmd.Parameters.Add(new SqlParameter("@PIVA", string.IsNullOrEmpty(cliente.PIVA) ? (object)DBNull.Value : cliente.PIVA));
                cmd.Parameters.Add(new SqlParameter("@Id", cliente.Id));

                cmd.Transaction = transaction; // Assegna la transazione al comando

                var result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new Exception("Aggiornamento Cliente non completato");
                }

                transaction.Commit(); // Conferma la transazione se tutto va bene
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Annulla la transazione in caso di errore
                throw new Exception("Errore durante l'aggiornamento del cliente", ex);
            }
        }

    }
}
