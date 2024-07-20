using Esercizio_M5_W1_D5.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Esercizio_M5_W1_D5.Services
{
    public class AnagraficaService : SqlServerServiceBase, IAnagraficaService
    {
        public AnagraficaService(IConfiguration config) : base(config) { }

        // metodo per aggiungere un nuovo trasgressore
        public void AddTrasgressore(Anagrafica trasgressore)
        {
            if (trasgressore == null)
            {
                throw new ArgumentNullException(nameof(trasgressore));
            }

            var query = @"
                INSERT INTO Anagrafiche (Cognome, Nome, Indirizzo, Città, CAP, CF)
                VALUES (@Cognome, @Nome, @Indirizzo, @Città, @CAP, @CF)";

            using (var conn = GetConnection())
            {
                using (var cmd = GetCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Cognome", trasgressore.Cognome));
                    cmd.Parameters.Add(new SqlParameter("@Nome", trasgressore.Nome));
                    cmd.Parameters.Add(new SqlParameter("@Indirizzo", trasgressore.Indirizzo));
                    cmd.Parameters.Add(new SqlParameter("@Città", trasgressore.Città));
                    cmd.Parameters.Add(new SqlParameter("@CAP", trasgressore.CAP));
                    cmd.Parameters.Add(new SqlParameter("@CF", trasgressore.CF));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // metodo per recuperare tutti i trasgressori e calcolarne sia il totale dei verbali che il totale dei punti decurtati
        public IEnumerable<Anagrafica> GetAllTrasgressore()
        {
            var query = @"
                SELECT a.IDAnagrafica, a.Cognome, a.Nome, a.Indirizzo, a.Città, a.CAP, a.CF, 
                COUNT(v.IDVerbale) AS TotaleVerbali,
                COALESCE(SUM(vi.DecurtamentoPunti), 0) AS TotalePuntiDecurtati
                FROM Anagrafiche a
                LEFT JOIN Verbali v ON a.IDAnagrafica = v.Anagrafica_FK
                LEFT JOIN Violazioni vi ON v.Violazione_FK = vi.IDViolazione
                GROUP BY a.IDAnagrafica, a.Cognome, a.Nome, a.Indirizzo, a.Città, a.CAP, a.CF";


            var listaTrasgressori = new List<Anagrafica>();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = GetCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaTrasgressori.Add(Create(reader));
                        }
                    }
                }
            }

            return listaTrasgressori;
        }

        //metodo per la creazione di una nuova Anagrafica presa dal DB
        private Anagrafica Create(DbDataReader reader)
        {
            return new Anagrafica
            {
                Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Indirizzo = reader.GetString(reader.GetOrdinal("Indirizzo")),
                Città = reader.GetString(reader.GetOrdinal("Città")),
                CAP = reader.GetString(reader.GetOrdinal("CAP")),
                CF = reader.GetString(reader.GetOrdinal("CF")),
                TotaleVerbali = reader.GetInt32(reader.GetOrdinal("TotaleVerbali")),
                TotalePuntiDecurtati = reader.GetInt32(reader.GetOrdinal("TotalePuntiDecurtati")),
                IDAnagrafica = reader.GetInt32(reader.GetOrdinal("IDAnagrafica"))

            };
        }
         
    }
}
