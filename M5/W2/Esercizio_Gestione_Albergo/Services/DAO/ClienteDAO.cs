using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Esercizio_Gestione_Albergo.Services.DAO;
using Esercizio_Gestione_Albergo.Services.Sql;
using Esercizio_Gestione_Albergo.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Esercizio_Gestione_Albergo.DataAccess
{
    public class ClienteDAO : SqlServerServiceBase, IClienteDAO
    {
        private const string GetAllQuery =              "SELECT * FROM Clienti";
        private const string GetByCodiceFiscaleQuery =  "SELECT * FROM Clienti WHERE CodiceFiscale = @CodiceFiscale";
        private const string InsertQuery =              "INSERT INTO Clienti (CodiceFiscale, Cognome, Nome, Città, Provincia, Email, " +
                                                        "Telefono, Cellulare) VALUES (@CodiceFiscale, @Cognome, @Nome, @Città, @Provincia, " +
                                                        "@Email, @Telefono, @Cellulare)";
        private const string UpdateQuery =              "UPDATE Clienti SET Cognome = @Cognome, Nome = @Nome, Città = @Città, " +
                                                        "Provincia = @Provincia, Email = @Email, Telefono = @Telefono, " +
                                                        "Cellulare = @Cellulare WHERE CodiceFiscale = @CodiceFiscale";
        private const string DeleteQuery =              "DELETE FROM Clienti WHERE CodiceFiscale = @CodiceFiscale";

        public ClienteDAO(IConfiguration config) : base(config) { }

        public IEnumerable<ClienteViewModel> GetAll()
        {
            var clienti = new List<ClienteViewModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = GetCommand(GetAllQuery, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cliente = new ClienteViewModel
                        {
                            CodiceFiscale =     reader["CodiceFiscale"]?.ToString(),
                            Cognome =           reader["Cognome"]?.ToString(),
                            Nome =              reader["Nome"]?.ToString(),
                            Città =             reader["Città"]?.ToString(),
                            Provincia =         reader["Provincia"]?.ToString(),
                            Email =             reader["Email"]?.ToString(),
                            Telefono =          reader["Telefono"]?.ToString(),
                            Cellulare =         reader["Cellulare"]?.ToString()
                        };
                        clienti.Add(cliente);
                    }
                }
            }
            return clienti;
        }

        public ClienteViewModel GetByCodiceFiscale(string codiceFiscale)
        {
            ClienteViewModel cliente = null;
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = GetCommand(GetByCodiceFiscaleQuery, connection);
                command.Parameters.Add(new SqlParameter("@CodiceFiscale", SqlDbType.VarChar, 16) { Value = codiceFiscale });

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new ClienteViewModel
                        {
                            CodiceFiscale =     reader["CodiceFiscale"].ToString(),
                            Cognome =           reader["Cognome"].ToString(),
                            Nome =              reader["Nome"].ToString(),
                            Città =             reader["Città"].ToString(),
                            Provincia =         reader["Provincia"].ToString(),
                            Email =             reader["Email"].ToString(),
                            Telefono =          reader["Telefono"].ToString(),
                            Cellulare =         reader["Cellulare"].ToString()
                        };
                    }
                }
            }
            return cliente;
        }

        public void Add(Cliente cliente)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = GetCommand(InsertQuery, connection);
                command.Parameters.Add(new SqlParameter("@CodiceFiscale", SqlDbType.VarChar, 16) { Value = cliente.CodiceFiscale });
                command.Parameters.Add(new SqlParameter("@Cognome", SqlDbType.NVarChar, 50) { Value = cliente.Cognome });
                command.Parameters.Add(new SqlParameter("@Nome", SqlDbType.NVarChar, 50) { Value = cliente.Nome });
                command.Parameters.Add(new SqlParameter("@Città", SqlDbType.NVarChar, 100) { Value = cliente.Città });
                command.Parameters.Add(new SqlParameter("@Provincia", SqlDbType.Char, 2) { Value = cliente.Provincia });
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = cliente.Email });
                command.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.NVarChar, 20) { Value = (object)cliente.Telefono ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Cellulare", SqlDbType.NVarChar, 20) { Value = (object)cliente.Cellulare ?? DBNull.Value });

                command.ExecuteNonQuery();
            }
        }

        public void Update(Cliente cliente)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = GetCommand(UpdateQuery, connection);
                command.Parameters.Add(new SqlParameter("@CodiceFiscale", SqlDbType.VarChar, 16) { Value = cliente.CodiceFiscale });
                command.Parameters.Add(new SqlParameter("@Cognome", SqlDbType.NVarChar, 50) { Value = cliente.Cognome });
                command.Parameters.Add(new SqlParameter("@Nome", SqlDbType.NVarChar, 50) { Value = cliente.Nome });
                command.Parameters.Add(new SqlParameter("@Città", SqlDbType.NVarChar, 100) { Value = cliente.Città });
                command.Parameters.Add(new SqlParameter("@Provincia", SqlDbType.Char, 2) { Value = cliente.Provincia });
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = cliente.Email });
                command.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.NVarChar, 20) { Value = (object)cliente.Telefono ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Cellulare", SqlDbType.NVarChar, 20) { Value = (object)cliente.Cellulare ?? DBNull.Value });

                command.ExecuteNonQuery();
            }
        }

        public void Delete(string codiceFiscale)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = GetCommand(DeleteQuery, connection);
                command.Parameters.Add(new SqlParameter("@CodiceFiscale", SqlDbType.VarChar, 16) { Value = codiceFiscale });

                command.ExecuteNonQuery();
            }
        }
    }
}
