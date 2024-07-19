using Esercizio_M5_W1_D5.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace Esercizio_M5_W1_D5.Services
{
    public class ViolazioneService : SqlServerServiceBase, IViolazioneService
    {
        public ViolazioneService(IConfiguration config) : base(config) { }
        public void AddViolazione(Violazione violazione)
        {
            if (violazione == null)
            {
                throw new ArgumentNullException(nameof(violazione));
            }

            var query = @"
                INSERT INTO Violazioni (Descrizione, Importo, DecrutamentoPUnti)
                VALUES (@Descrizione, @Importo, @DecurtamentoPunti)";

            using (var conn = GetConnection())
            {
                using (var cmd = GetCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Descrizione", violazione.Descrizione));
                    cmd.Parameters.Add(new SqlParameter("@Importo", violazione.Importo));
                    cmd.Parameters.Add(new SqlParameter("@DecurtamentoPunti", violazione.DecurtamentoPunti));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Violazione> GetAllViolazioni()
        {
            var query = "SELECT * FROM Violazioni";
            var listaViolazioni = new List<Violazione>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaViolazioni.Add(Create(reader));
                        }
                    }
                }
            }

            return listaViolazioni;
        }

        private Violazione Create(DbDataReader reader)
        {
            return new Violazione
            {
                Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti"))
            };
        }

        public IEnumerable<ViolazioneDettaglio> GetViolazioniConImportoMaggioreDi400()
        {
            var query = @"
                SELECT 
                    a.Cognome, 
                    a.Nome, 
                    v.DataViolazione, 
                    vi.Importo, 
                    vi.DecurtamentoPunti,
                    vi.Descrizione
                FROM Anagrafiche a
                INNER JOIN Verbali v ON a.IDAnagrafica = v.Anagrafica_FK
                INNER JOIN Violazioni vi ON v.Violazione_FK = vi.IDViolazione
                WHERE vi.Importo > 400";

            var listaViolazioni = new List<ViolazioneDettaglio>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var violazioneDettaglio = new ViolazioneDettaglio
                            {
                                Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                DataViolazione = reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                                Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                                DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti")),
                                Descrizione = reader.GetString(reader.GetOrdinal("Descrizione"))
                            };
                            listaViolazioni.Add(violazioneDettaglio);
                        }
                    }
                }
            }

            return listaViolazioni;
        }
        public IEnumerable<ViolazioneDettaglio> GetViolazioniConPuntiSuperioriA10()
        {
            var query = @"
                SELECT 
                    a.Cognome, 
                    a.Nome, 
                    v.DataViolazione, 
                    vi.Importo, 
                    vi.DecurtamentoPunti,
                    vi.Descrizione
                FROM Anagrafiche a
                INNER JOIN Verbali v ON a.IDAnagrafica = v.Anagrafica_FK
                INNER JOIN Violazioni vi ON v.Violazione_FK = vi.IDViolazione
                WHERE vi.DecurtamentoPunti > 10";

            var listaViolazioni = new List<ViolazioneDettaglio>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var violazioneDettaglio = new ViolazioneDettaglio
                            {
                                Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                DataViolazione = reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                                Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                                DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti")),
                                Descrizione = reader.GetString(reader.GetOrdinal("Descrizione"))
                            };
                            listaViolazioni.Add(violazioneDettaglio);
                        }
                    }
                }
            }

            return listaViolazioni;
        }
    }
}
