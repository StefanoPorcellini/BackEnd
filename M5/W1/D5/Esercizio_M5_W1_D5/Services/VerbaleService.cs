using Esercizio_M5_W1_D5.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Esercizio_M5_W1_D5.Services
{
    public class VerbaleService : SqlServerServiceBase, IVerbaleService
    {
        private readonly ILogger<VerbaleService> _logger;
        public VerbaleService(IConfiguration config, ILogger<VerbaleService> logger) : base(config)
        {
            _logger = logger;
        }

        //metodi per creare un nuovo verbale
        public void AddVerbale(Verbale verbale)
        {
            if (verbale == null)
            {
                throw new ArgumentNullException(nameof(verbale));
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

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        _logger.LogInformation($"Rows affected: {rowsAffected}"); 
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error adding verbale: {ex.Message}");
                        throw;
                    }
                }
            }
        }


        //Metodo per recuperare tutti i verbali dal DB
        public IEnumerable<Verbale> GetAllVerbali()
        {
            var query = @"
                        SELECT v.*, 
                        a.IDAnagrafica, a.Cognome, a.Nome, a.Indirizzo, a.Città, a.CAP, a.CF,
                        vi.Descrizione, vi.Importo, vi.DecurtamentoPunti
                        FROM Verbali v
                        JOIN Anagrafiche a ON v.Anagrafica_FK = a.IDAnagrafica
                        JOIN Violazioni vi ON v.Violazione_FK = vi.IDViolazione";

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

        //metodo per la creazione di un nuovo Verbale preso dal DB (Prendo anche i dati relativi alla chiave esterna usarli nei dettagli del verbale)
        private VerbaleViewModel Create(DbDataReader reader)
        {
            return new VerbaleViewModel
            {   
                IDVerbale = reader.GetInt32(reader.GetOrdinal("IDVerbale")),
                DataViolazione = reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                IndirizzoViolazione = reader.GetString(reader.GetOrdinal("IndirizzoViolazione")),
                NominativoAgente = reader.GetString(reader.GetOrdinal("NominativoAgente")),
                DataTrascrizioneVerbale = reader.GetDateTime(reader.GetOrdinal("DataTrascrizioneVerbale")),
                Anagrafica_FK = reader.GetInt32(reader.GetOrdinal("Anagrafica_FK")),
                Violazione_FK = reader.GetInt32(reader.GetOrdinal("Violazione_FK")),
                Trasgressore = new Anagrafica
                {
                    IDAnagrafica = reader.GetInt32(reader.GetOrdinal("IDAnagrafica")),
                    Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                    Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo")),
                    Città = reader.GetString(reader.GetOrdinal("Città")),
                    CAP = reader.GetString(reader.GetOrdinal("CAP")),
                    CF = reader.GetString(reader.GetOrdinal("CF"))
                },
                Violazione = new Violazione
                {
                    IDViolazione = reader.GetInt32(reader.GetOrdinal("Violazione_FK")),
                    Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                    Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                    DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti"))
                }
            };
        }

        //metodo che prende i dati del verbale in base al ID del Anagrafica
        public IEnumerable<Verbale> GetByAnagraficaId(int anagraficaId)
        {
            var query = @"
                        SELECT v.*, 
                        a.IDAnagrafica, a.Cognome, a.Nome, a.Indirizzo, a.Città, a.CAP, a.CF,
                        vi.Descrizione, vi.Importo, vi.DecurtamentoPunti
                        FROM Verbali v
                        JOIN Anagrafiche a ON v.Anagrafica_FK = a.IDAnagrafica
                        JOIN Violazioni vi ON v.Violazione_FK = vi.IDViolazione
                        WHERE v.Anagrafica_FK = @AnagraficaId";

            var listaVerbali = new List<Verbale>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@AnagraficaId", anagraficaId));

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

        //metodo per prendere i dati di un verbale in base al suo ID
        public Verbale GetById(int id)
        {
            var query = @"
                        SELECT v.*, 
                        a.IDAnagrafica, a.Cognome, a.Nome, a.Indirizzo, a.Città, a.CAP, a.CF,
                        vi.Descrizione, vi.Importo, vi.DecurtamentoPunti
                        FROM Verbali v
                        JOIN Anagrafiche a ON v.Anagrafica_FK = a.IDAnagrafica
                        JOIN Violazioni vi ON v.Violazione_FK = vi.IDViolazione
                        WHERE v.IDVerbale = @Id";

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Create(reader);
                        }
                    }
                }
            }
            return null;
        }

        //Metodo per prelevare i dati delle violazioni il cui decurtamento punti supera i 10 punti
        public IEnumerable<ViolazioneDettaglio> GetViolazioniOltre10Punti()
        {
            var query = @"
                        SELECT 
                            v.Importo,
                            a.Cognome,
                            a.Nome,
                            ve.DataViolazione,
                            v.DecurtamentoPunti
                        FROM Violazioni v
                        JOIN Verbali ve ON v.IDViolazione = ve.Violazione_FK
                        JOIN Anagrafiche a ON ve.Anagrafica_FK = a.IDAnagrafica
                        WHERE v.DecurtamentoPunti > 10";

            var violazioniDetails = new List<ViolazioneDettaglio>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var violazioneDetail = new ViolazioneDettaglio
                            {
                                Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                                Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                DataViolazione = reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                                DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti"))
                            };
                            violazioniDetails.Add(violazioneDetail);
                        }
                    }
                }
            }

            return violazioniDetails;
        }

        //Metodo per prelevare i dati delle violazioni che superano l'importo di 400€
        public IEnumerable<ViolazioneDettaglio> GetViolazioniOltre400Euro()
        {
            var query = @"
                        SELECT 
                            v.Importo,
                            a.Cognome,
                            a.Nome,
                            ve.DataViolazione,
                            v.DecurtamentoPunti
                        FROM Violazioni v
                        JOIN Verbali ve ON v.IDViolazione = ve.Violazione_FK
                        JOIN Anagrafiche a ON ve.Anagrafica_FK = a.IDAnagrafica
                        WHERE v.Importo > 400";

            var violazioniDetails = new List<ViolazioneDettaglio>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var violazioneDetail = new ViolazioneDettaglio
                            {
                                Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                                Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                DataViolazione = reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                                DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti"))
                            };
                            violazioniDetails.Add(violazioneDetail);
                        }
                    }
                }
            }

            return violazioniDetails;
        }

    }
}
