using System.Data;
using System.Data.SqlClient;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.DAO;
using Esercizio_Gestione_Albergo.Services.Sql;
using Esercizio_Gestione_Albergo.ViewModels;

namespace Esercizio_Gestione_Albergo.DataAccess
{
    public class PrenotazioneDAO : SqlServerServiceBase, IPrenotazioneDAO
    {
        private readonly ILogger<PrenotazioneDAO> _logger;

        public PrenotazioneDAO(IConfiguration config, ILogger<PrenotazioneDAO> logger) : base(config)
        {
            _logger = logger;
        }

        private const string GetAllQuery = "SELECT * FROM Prenotazioni";
        private const string GetByIdQuery = "SELECT * FROM Prenotazioni WHERE ID = @ID";
        private const string InsertQuery = "INSERT INTO Prenotazioni (ClienteCodiceFiscale, CameraNumero, DataPrenotazione, NumeroProgressivo, Anno, Dal, Al, CaparraConfirmatoria, Tariffa, DettagliSoggiornoId, SaldoFinale) " +
                                            "VALUES (@ClienteCodiceFiscale, @CameraNumero, GETDATE(), @NumeroProgressivo, @Anno, @Dal, @Al, @CaparraConfirmatoria, @Tariffa, @DettagliSoggiornoId, @SaldoFinale); SELECT SCOPE_IDENTITY();";

        private const string UpdateQuery = "UPDATE Prenotazioni SET ClienteCodiceFiscale = @ClienteCodiceFiscale, CameraNumero = @CameraNumero, " +
                                            "DataPrenotazione = @DataPrenotazione, NumeroProgressivo = @NumeroProgressivo, Anno = @Anno, Dal = @Dal, " +
                                            "Al = @Al, CaparraConfirmatoria = @CaparraConfirmatoria, Tariffa = @Tariffa, SaldoFinale = @SaldoFinale, " +
                                            "DettagliSoggiornoId = @DettagliSoggiornoId WHERE ID = @ID";

        private const string DeleteQuery = "DELETE FROM Prenotazioni WHERE ID = @ID";

        private const string CheckAvailabilityQuery = @"SELECT COUNT(*) 
                                                        FROM Prenotazioni 
                                                        WHERE CameraNumero = @CameraNumero 
                                                        AND (
                                                            (@Dal BETWEEN Dal AND Al) 
                                                            OR (@Al BETWEEN Dal AND Al)
                                                            OR (Dal BETWEEN @Dal AND @Al)
                                                            OR (Al BETWEEN @Dal AND @Al)
                                                            )";

        private const string UpdateCameraAvailabilityQuery = "UPDATE Camere SET Disponibile = @Disponibile WHERE Numero = @Numero";

        private const string CalcoloTariffa = "SELECT (ds.Prezzo + (tc.Prezzo * c.Coefficiente) * @Giorni) AS Tariffa " +
                                               "FROM Camere c " +
                                               "JOIN TipologieCamere tc ON c.TipologiaId = tc.Id " +
                                               "JOIN DettagliSoggiorno ds ON ds.Id = @DettaglioSoggiornoId " +
                                               "WHERE c.Numero = @CameraNumero";

        public async Task<IEnumerable<PrenotazioneViewModel>> GetAllAsync()
        {
            var prenotazioni = new List<PrenotazioneViewModel>();
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(GetAllQuery, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        prenotazioni.Add(new PrenotazioneViewModel
                        {
                            ID = (int)reader["ID"],
                            ClienteCodiceFiscale = reader["ClienteCodiceFiscale"].ToString(),
                            CameraNumero = (int)reader["CameraNumero"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            NumeroProgressivo = (string)reader["NumeroProgressivo"],
                            Anno = (int)reader["Anno"],
                            Dal = (DateTime)reader["Dal"],
                            Al = (DateTime)reader["Al"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            DettagliSoggiornoId = (int)reader["DettagliSoggiornoId"],
                            SaldoFinale = (decimal)reader["SaldoFinale"]
                        });
                    }
                }
            }
            return prenotazioni;
        }

        public async Task<PrenotazioneViewModel> GetByIdAsync(int id)
        {
            PrenotazioneViewModel prenotazione = null;
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(GetByIdQuery, connection);
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = id });

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        prenotazione = new PrenotazioneViewModel
                        {
                            ID = (int)reader["ID"],
                            ClienteCodiceFiscale = reader["ClienteCodiceFiscale"].ToString(),
                            CameraNumero = (int)reader["CameraNumero"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            NumeroProgressivo = (string)reader["NumeroProgressivo"],
                            Anno = (int)reader["Anno"],
                            Dal = (DateTime)reader["Dal"],
                            Al = (DateTime)reader["Al"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            DettagliSoggiornoId = (int)reader["DettagliSoggiornoId"],
                            SaldoFinale = (decimal)reader["SaldoFinale"]
                        };
                    }
                }
            }
            return prenotazione;
        }

        public async Task AddAsync(Prenotazione prenotazione)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var transaction = connection.BeginTransaction();
                try
                {
                    int anno = prenotazione.Dal.Year;

                    var command = GetCommand(InsertQuery, connection);
                    command.Transaction = transaction;

                    command.Parameters.Add(new SqlParameter("@ClienteCodiceFiscale", SqlDbType.VarChar, 16) { Value = prenotazione.ClienteCodiceFiscale });
                    command.Parameters.Add(new SqlParameter("@CameraNumero", SqlDbType.Int) { Value = prenotazione.CameraNumero });
                    command.Parameters.Add(new SqlParameter("@NumeroProgressivo", SqlDbType.VarChar, 15) { Value = DBNull.Value }); // Placeholder
                    command.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int) { Value = anno });
                    command.Parameters.Add(new SqlParameter("@Dal", SqlDbType.Date) { Value = prenotazione.Dal });
                    command.Parameters.Add(new SqlParameter("@Al", SqlDbType.Date) { Value = prenotazione.Al });
                    command.Parameters.Add(new SqlParameter("@CaparraConfirmatoria", SqlDbType.Decimal) { Value = prenotazione.CaparraConfirmatoria });
                    command.Parameters.Add(new SqlParameter("@Tariffa", SqlDbType.Decimal) { Value = prenotazione.Tariffa });
                    command.Parameters.Add(new SqlParameter("@DettagliSoggiornoId", SqlDbType.Int) { Value = prenotazione.DettagliSoggiornoId });
                    command.Parameters.Add(new SqlParameter("@SaldoFinale", SqlDbType.Decimal) { Value = prenotazione.SaldoFinale });

                    _logger.LogDebug("Esecuzione della query di inserimento con i seguenti parametri: ClienteCodiceFiscale={ClienteCodiceFiscale}, CameraNumero={CameraNumero}, Anno={Anno}, Dal={Dal}, Al={Al}, CaparraConfirmatoria={CaparraConfirmatoria}, Tariffa={Tariffa}, DettagliSoggiornoId={DettagliSoggiornoId}, SaldoFinale={SaldoFinale}",
                        prenotazione.ClienteCodiceFiscale, prenotazione.CameraNumero, anno, prenotazione.Dal, prenotazione.Al, prenotazione.CaparraConfirmatoria, prenotazione.Tariffa, prenotazione.DettagliSoggiornoId, prenotazione.SaldoFinale);

                    var result = await command.ExecuteScalarAsync();
                    var prenotazioneId = Convert.ToInt32(result);

                    _logger.LogDebug("ID della prenotazione inserita: {PrenotazioneId}", prenotazioneId);


                    var availabilityCommand = GetCommand(UpdateCameraAvailabilityQuery, connection);
                    availabilityCommand.Transaction = transaction;
                    availabilityCommand.Parameters.Add(new SqlParameter("@Disponibile", SqlDbType.Bit) { Value = false });
                    availabilityCommand.Parameters.Add(new SqlParameter("@Numero", SqlDbType.Int) { Value = prenotazione.CameraNumero });

                    await availabilityCommand.ExecuteNonQueryAsync();

                    transaction.Commit();

                    _logger.LogInformation("Prenotazione aggiunta e numero progressivo aggiornato con successo: ID={PrenotazioneId}", prenotazioneId);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "Errore durante l'aggiunta della prenotazione.");
                    throw;
                }
            }
        }

        public async Task UpdateAsync(PrenotazioneViewModel prenotazione)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(UpdateQuery, connection);
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = prenotazione.ID });
                command.Parameters.Add(new SqlParameter("@ClienteCodiceFiscale", SqlDbType.VarChar, 16) { Value = prenotazione.ClienteCodiceFiscale });
                command.Parameters.Add(new SqlParameter("@CameraNumero", SqlDbType.Int) { Value = prenotazione.CameraNumero });
                command.Parameters.Add(new SqlParameter("@DataPrenotazione", SqlDbType.DateTime2) { Value = prenotazione.DataPrenotazione });
                command.Parameters.Add(new SqlParameter("@NumeroProgressivo", SqlDbType.VarChar, 15) { Value = prenotazione.NumeroProgressivo });
                command.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int) { Value = prenotazione.Anno });
                command.Parameters.Add(new SqlParameter("@Dal", SqlDbType.Date) { Value = prenotazione.Dal });
                command.Parameters.Add(new SqlParameter("@Al", SqlDbType.Date) { Value = prenotazione.Al });
                command.Parameters.Add(new SqlParameter("@CaparraConfirmatoria", SqlDbType.Decimal) { Value = prenotazione.CaparraConfirmatoria });
                command.Parameters.Add(new SqlParameter("@Tariffa", SqlDbType.Decimal) { Value = prenotazione.Tariffa });
                command.Parameters.Add(new SqlParameter("@DettagliSoggiornoId", SqlDbType.Int) { Value = prenotazione.DettagliSoggiornoId });
                command.Parameters.Add(new SqlParameter("@SaldoFinale", SqlDbType.Decimal) { Value = prenotazione.SaldoFinale });

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(DeleteQuery, connection);
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = id });

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> IsCameraAvailableAsync(int cameraNumero, DateTime dal, DateTime al)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(CheckAvailabilityQuery, connection);

                command.Parameters.Add(new SqlParameter("@CameraNumero", SqlDbType.Int) { Value = cameraNumero });
                command.Parameters.Add(new SqlParameter("@Dal", SqlDbType.Date) { Value = dal });
                command.Parameters.Add(new SqlParameter("@Al", SqlDbType.Date) { Value = al });

                var count = (int)await command.ExecuteScalarAsync();
                return count == 0;
            }
        }

        public async Task<decimal> CalcoloTariffaAsync(int cameraNumero, int dettaglioSoggiornoId, int giorni)
        {
            decimal tariffa = 0;
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(CalcoloTariffa, connection);
                command.Parameters.Add(new SqlParameter("@Giorni", SqlDbType.Int) { Value = giorni });
                command.Parameters.Add(new SqlParameter("@CameraNumero", SqlDbType.Int) { Value = cameraNumero });
                command.Parameters.Add(new SqlParameter("@DettaglioSoggiornoId", SqlDbType.Int) { Value = dettaglioSoggiornoId });

                tariffa = (decimal)await command.ExecuteScalarAsync();
            }
            return tariffa;
        }
    }
}
