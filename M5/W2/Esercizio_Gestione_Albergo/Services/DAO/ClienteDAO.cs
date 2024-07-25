using System.Data;
using System.Data.SqlClient;
using Esercizio_Gestione_Albergo.Services.DAO;
using Esercizio_Gestione_Albergo.Services.Sql;
using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.DataAccess
{
    public class ClienteDAO : SqlServerServiceBase, IClienteDAO
    {
        private const string GetAllQuery = "SELECT * FROM Clienti";
        private const string GetByCodiceFiscaleQuery = @"
            SELECT c.CodiceFiscale, c.Cognome, c.Nome, c.Città, c.Provincia, c.Email, c.Telefono, c.Cellulare,
                   p.ID, p.DataPrenotazione, p.CameraNumero, p.Tariffa, p.CaparraConfirmatoria
            FROM Clienti c
            LEFT JOIN Prenotazioni p ON c.CodiceFiscale = p.ClienteCodiceFiscale
            WHERE c.CodiceFiscale = @CodiceFiscale";
        private const string InsertQuery = "INSERT INTO Clienti (CodiceFiscale, Cognome, Nome, Città, Provincia, Email, " +
                                           "Telefono, Cellulare) VALUES (@CodiceFiscale, @Cognome, @Nome, @Città, @Provincia, " +
                                           "@Email, @Telefono, @Cellulare)";
        private const string UpdateQuery = "UPDATE Clienti SET Cognome = @Cognome, Nome = @Nome, Città = @Città, " +
                                           "Provincia = @Provincia, Email = @Email, Telefono = @Telefono, " +
                                           "Cellulare = @Cellulare WHERE CodiceFiscale = @CodiceFiscale";
        private const string DeleteQuery = "DELETE FROM Clienti WHERE CodiceFiscale = @CodiceFiscale";

        public ClienteDAO(IConfiguration config) : base(config) { }

        public async Task<IEnumerable<ClienteViewModel>> GetAll()
        {
            var clienti = new List<ClienteViewModel>();
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(GetAllQuery, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var cliente = new ClienteViewModel
                        {
                            CodiceFiscale = reader["CodiceFiscale"]?.ToString(),
                            Cognome = reader["Cognome"]?.ToString(),
                            Nome = reader["Nome"]?.ToString(),
                            Città = reader["Città"]?.ToString(),
                            Provincia = reader["Provincia"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            Telefono = reader["Telefono"]?.ToString(),
                            Cellulare = reader["Cellulare"]?.ToString(),
                            Prenotazioni = new List<PrenotazioneViewModel>()
                        };
                        clienti.Add(cliente);
                    }
                }
            }
            return clienti;
        }

        public async Task<ClienteViewModel> GetByCodiceFiscale(string codiceFiscale)
        {
            ClienteViewModel cliente = null;
            var prenotazioni = new List<PrenotazioneViewModel>();

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(GetByCodiceFiscaleQuery, connection);
                command.Parameters.Add(new SqlParameter("@CodiceFiscale", SqlDbType.VarChar, 16) { Value = codiceFiscale });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Se il cliente non è ancora stato creato, crealo
                        if (cliente == null)
                        {
                            cliente = new ClienteViewModel
                            {
                                CodiceFiscale = reader["CodiceFiscale"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Città = reader["Città"].ToString(),
                                Provincia = reader["Provincia"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Cellulare = reader["Cellulare"].ToString(),
                                Prenotazioni = new List<PrenotazioneViewModel>()
                            };
                        }

                        // Aggiungi le prenotazioni se esistono
                        if (reader["ID"] != DBNull.Value)
                        {
                            var prenotazione = new PrenotazioneViewModel
                            {
                                DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                                CameraNumero = reader.GetInt32(reader.GetOrdinal("CameraNumero")),
                                Tariffa = reader.GetDecimal(reader.GetOrdinal("Tariffa")),
                                CaparraConfirmatoria = reader.GetDecimal(reader.GetOrdinal("CaparraConfirmatoria"))
                            };

                            cliente.Prenotazioni.Add(prenotazione);
                        }
                    }
                }
            }
            return cliente;
        }

        public async Task Add(Cliente cliente)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(InsertQuery, connection);
                command.Parameters.Add(new SqlParameter("@CodiceFiscale", SqlDbType.VarChar, 16) { Value = cliente.CodiceFiscale });
                command.Parameters.Add(new SqlParameter("@Cognome", SqlDbType.NVarChar, 50) { Value = cliente.Cognome });
                command.Parameters.Add(new SqlParameter("@Nome", SqlDbType.NVarChar, 50) { Value = cliente.Nome });
                command.Parameters.Add(new SqlParameter("@Città", SqlDbType.NVarChar, 100) { Value = cliente.Città });
                command.Parameters.Add(new SqlParameter("@Provincia", SqlDbType.Char, 2) { Value = cliente.Provincia });
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = cliente.Email });
                command.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.NVarChar, 20) { Value = (object)cliente.Telefono ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Cellulare", SqlDbType.NVarChar, 20) { Value = (object)cliente.Cellulare ?? DBNull.Value });

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Update(Cliente cliente)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(UpdateQuery, connection);
                command.Parameters.Add(new SqlParameter("@CodiceFiscale", SqlDbType.VarChar, 16) { Value = cliente.CodiceFiscale });
                command.Parameters.Add(new SqlParameter("@Cognome", SqlDbType.NVarChar, 50) { Value = cliente.Cognome });
                command.Parameters.Add(new SqlParameter("@Nome", SqlDbType.NVarChar, 50) { Value = cliente.Nome });
                command.Parameters.Add(new SqlParameter("@Città", SqlDbType.NVarChar, 100) { Value = cliente.Città });
                command.Parameters.Add(new SqlParameter("@Provincia", SqlDbType.Char, 2) { Value = cliente.Provincia });
                command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = cliente.Email });
                command.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.NVarChar, 20) { Value = (object)cliente.Telefono ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Cellulare", SqlDbType.NVarChar, 20) { Value = (object)cliente.Cellulare ?? DBNull.Value });

                await command.ExecuteNonQueryAsync();
            }
        }


        public async Task Delete(string codiceFiscale)
        {
            // Verifica se il parametro non è nullo o vuoto
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                throw new ArgumentException("Il codice fiscale non può essere nullo o vuoto.", nameof(codiceFiscale));
            }


            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var command = GetCommand(DeleteQuery, connection))
                {
                    // Aggiungi il parametro al comando
                    command.Parameters.Add(new SqlParameter("@CodiceFiscale", SqlDbType.VarChar, 16) { Value = codiceFiscale });

                    try
                    {
                        // Esegui la query
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected == 0)
                        {
                            // Opzionale: Puoi gestire il caso in cui nessun record è stato eliminato
                            throw new InvalidOperationException("Nessun cliente trovato con il codice fiscale specificato.");
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Gestisci eccezioni di SQL
                        Console.WriteLine($"Si è verificato un errore durante l'eliminazione del cliente: {ex.Message}");
                        throw; // Rilancia l'eccezione per la gestione a un livello superiore
                    }
                }
            }
        }

    }
}
