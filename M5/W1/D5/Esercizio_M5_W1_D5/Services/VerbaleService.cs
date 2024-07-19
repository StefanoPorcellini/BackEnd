using Esercizio_M5_W1_D5.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.Common;


namespace Esercizio_M5_W1_D5.Services
{
    public class VerbaleService : SqlServerServiceBase, IVerbaleService
    {
        public VerbaleService(IConfiguration config) : base(config) { }

        public void AddVerbale(Verbale verbale)
        {
            if (verbale == null)
            {
                throw new ArgumentNullException(nameof(verbale));
            }
            // Verifica che l'Anagrafica_FK esista
            if (!DoesAnagraficaExist(verbale.Anagrafica_FK))
            {
                throw new ArgumentException("L'Anagrafica_FK specificato non esiste.");
            }

            // Verifica che la Violazione_FK esista
            if (!DoesViolazioneExist(verbale.Violazione_FK))
            {
                throw new ArgumentException("La Violazione_FK specificata non esiste.");
            }
            var query = @"
                INSERT INTO Verbali (DataViolazione, IndirizzoViolazione, NominativoAgente, DataTrascrizioneVerbale, Anagrafica_FK, Violazione_FK)
                VALUES (@DataViolazione, @IndirizzoViolazione, @NominativoAgente, @DataTrascrizioneVerbale, @Anagrafica_FK, @Violazione_FK)";

            using (var conn = GetConnection())
            {
                using (var cmd = GetCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@DataViolazione", verbale.DataViolazione));
                    cmd.Parameters.Add(new SqlParameter("@IndirizzoViolazione", verbale.IndirizzoViolazione ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@NominativoAgente", verbale.NominativoAgente ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale));
                    cmd.Parameters.Add(new SqlParameter("@Anagrafica_FK", verbale.Anagrafica_FK));
                    cmd.Parameters.Add(new SqlParameter("@Violazione_FK", verbale.Violazione_FK));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //bool per la verifica del parametro della chiave esterna legata all'anagrafe
        private bool DoesAnagraficaExist(int anagraficaId)
        {
            var query = "SELECT COUNT(*) FROM Anagrafiche WHERE IDAnagrafica = @IDAnagrafica";
            using (var conn = GetConnection())
            {
                using (var cmd = GetCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@IDAnagrafica", anagraficaId));
                    conn.Open();
                    var count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        //bool per la verifica del parametro della chiave esterna legata alla violazione
        private bool DoesViolazioneExist(int violazioneId)
        {
            var query = "SELECT COUNT(*) FROM Violazioni WHERE IDViolazione = @IDViolazione";
            using (var conn = GetConnection())
            {
                using (var cmd = GetCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@IDViolazione", violazioneId));
                    conn.Open();
                    var count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        public IEnumerable<Verbale> GetAllVerbali()
        {
            var query = "SELECT * FROM Verbali";
            var listaVerbali = new List<Verbale>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaVerbali.Add(Create(reader));
                        }
                    }
                }
            }

            return listaVerbali;
        }

        private Verbale Create(DbDataReader reader)
        {
            return new Verbale
            {
                DataViolazione = reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                IndirizzoViolazione = reader.GetString(reader.GetOrdinal("IndirizzoViolazione")),
                NominativoAgente = reader.GetString(reader.GetOrdinal("NominativoAgente")),
                DataTrascrizioneVerbale = reader.GetDateTime(reader.GetOrdinal("DataTrascrizioneVerbale")),
                Anagrafica_FK = reader.GetInt32(reader.GetOrdinal("Anagrafica_FK")),
                Violazione_FK = reader.GetInt32(reader.GetOrdinal("Violazione_FK"))
            };
        }
    }
}
