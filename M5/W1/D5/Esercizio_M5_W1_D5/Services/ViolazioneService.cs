using Esercizio_M5_W1_D5.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace Esercizio_M5_W1_D5.Services
{
    public class ViolazioneService : SqlServerServiceBase, IViolazioneService
    {
        public ViolazioneService(IConfiguration config) : base(config) { }

        //Metodo per aggiungere una nuova violazione
        public void AddViolazione(Violazione violazione)
        {
            if (violazione == null)
            {
                throw new ArgumentNullException(nameof(violazione));
            }

            var query = @"
                INSERT INTO Violazioni (Descrizione, Importo, DecurtamentoPunti)
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

        //Metodo per prelevare tutte le violazioni dal database
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

        //Metodo per la creazione di una nuova violazione
        private Violazione Create(DbDataReader reader)
        {
            return new Violazione
            {
                Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti")),
                IDViolazione = reader.GetInt32(reader.GetOrdinal("IDViolazione"))
            };
        }
    }
}
