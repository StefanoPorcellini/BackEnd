using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.Sql;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public class DettagliSoggiornoDAO : SqlServerServiceBase, IDettaglioSoggiornoDAO
    {
        private const string GetAllQuery = "SELECT * FROM DettagliSoggiorno";

        public DettagliSoggiornoDAO(IConfiguration config) : base(config) { }

        public IEnumerable<DettagliSoggiorno> GetAll()
        {
            var dettagli = new List<DettagliSoggiorno>();
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = GetCommand(GetAllQuery, connection);

                if (command == null)
                {
                    throw new NullReferenceException("Failed to create SQL command.");
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dettagli.Add(new DettagliSoggiorno
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                            Descrizione = reader.IsDBNull(reader.GetOrdinal("Descrizione")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descrizione"))
                        });
                    }
                }
            }
            return dettagli;
        }
    }
}
