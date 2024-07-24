using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.DAO;
using Esercizio_Gestione_Albergo.Services.Sql;
using Esercizio_Gestione_Albergo.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Esercizio_Gestione_Albergo.DataAccess
{
    public class PrenotazioneDAO : SqlServerServiceBase, IPrenotazioneDAO
    {
        private const string GetAllQuery =  "SELECT * FROM Prenotazioni";

        private const string GetByIdQuery = "SELECT * FROM Prenotazioni WHERE ID = @ID";

        private const string InsertQuery =  "INSERT INTO Prenotazioni (ClienteCodiceFiscale, CameraNumero, DataPrenotazione, NumeroProgressivo, " +
                                            "Anno, Dal, Al, CaparraConfirmatoria, Tariffa, DettagliSoggiornoId) VALUES (@ClienteCodiceFiscale, " +
                                            "@CameraNumero, @DataPrenotazione, @NumeroProgressivo, @Anno, @Dal, @Al, @CaparraConfirmatoria, @Tariffa, " +
                                            "@DettagliSoggiornoId)";

        private const string UpdateQuery =  "UPDATE Prenotazioni SET ClienteCodiceFiscale = @ClienteCodiceFiscale, CameraNumero = @CameraNumero, " +
                                            "DataPrenotazione = @DataPrenotazione, NumeroProgressivo = @NumeroProgressivo, Anno = @Anno, Dal = @Dal, " +
                                            "Al = @Al, CaparraConfirmatoria = @CaparraConfirmatoria, Tariffa = @Tariffa, " +
                                            "DettagliSoggiornoId = @DettagliSoggiornoId WHERE ID = @ID";

        private const string DeleteQuery =  "DELETE FROM Prenotazioni WHERE ID = @ID";

        public PrenotazioneDAO(IConfiguration config) : base(config) { }

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
                            NumeroProgressivo = (int)reader["NumeroProgressivo"],
                            Anno = (int)reader["Anno"],
                            Dal = (DateTime)reader["Dal"],
                            Al = (DateTime)reader["Al"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            DettagliSoggiornoId = (int)reader["DettagliSoggiornoId"]
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
                            NumeroProgressivo = (int)reader["NumeroProgressivo"],
                            Anno = (int)reader["Anno"],
                            Dal = (DateTime)reader["Dal"],
                            Al = (DateTime)reader["Al"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            DettagliSoggiornoId = (int)reader["DettagliSoggiornoId"]
                        };
                    }
                }
            }
            return prenotazione;
        }

        public async Task AddAsync(PrenotazioneViewModel prenotazione)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var command = GetCommand(InsertQuery, connection);
                command.Parameters.Add(new SqlParameter("@ClienteCodiceFiscale", SqlDbType.VarChar, 16) { Value = prenotazione.ClienteCodiceFiscale });
                command.Parameters.Add(new SqlParameter("@CameraNumero", SqlDbType.Int) { Value = prenotazione.CameraNumero });
                command.Parameters.Add(new SqlParameter("@DataPrenotazione", SqlDbType.DateTime2) { Value = prenotazione.DataPrenotazione });
                command.Parameters.Add(new SqlParameter("@NumeroProgressivo", SqlDbType.Int) { Value = prenotazione.NumeroProgressivo });
                command.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int) { Value = prenotazione.Anno });
                command.Parameters.Add(new SqlParameter("@Dal", SqlDbType.Date) { Value = prenotazione.Dal });
                command.Parameters.Add(new SqlParameter("@Al", SqlDbType.Date) { Value = prenotazione.Al });
                command.Parameters.Add(new SqlParameter("@CaparraConfirmatoria", SqlDbType.Decimal) { Value = prenotazione.CaparraConfirmatoria });
                command.Parameters.Add(new SqlParameter("@Tariffa", SqlDbType.Decimal) { Value = prenotazione.Tariffa });
                command.Parameters.Add(new SqlParameter("@DettagliSoggiornoId", SqlDbType.Int) { Value = prenotazione.DettagliSoggiornoId });

                await command.ExecuteNonQueryAsync();
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
                command.Parameters.Add(new SqlParameter("@NumeroProgressivo", SqlDbType.Int) { Value = prenotazione.NumeroProgressivo });
                command.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int) { Value = prenotazione.Anno });
                command.Parameters.Add(new SqlParameter("@Dal", SqlDbType.Date) { Value = prenotazione.Dal });
                command.Parameters.Add(new SqlParameter("@Al", SqlDbType.Date) { Value = prenotazione.Al });
                command.Parameters.Add(new SqlParameter("@CaparraConfirmatoria", SqlDbType.Decimal) { Value = prenotazione.CaparraConfirmatoria });
                command.Parameters.Add(new SqlParameter("@Tariffa", SqlDbType.Decimal) { Value = prenotazione.Tariffa });
                command.Parameters.Add(new SqlParameter("@DettagliSoggiornoId", SqlDbType.Int) { Value = prenotazione.DettagliSoggiornoId });

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
    }
}
