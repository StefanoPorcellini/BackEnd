using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.Sql;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public class TipologiaCameraDAO : SqlServerServiceBase, ITipologiaCameraDAO
    {
        private const string GetAllQuery = "SELECT * FROM TipologieCamere";
        public TipologiaCameraDAO(IConfiguration config) : base(config) { }


        public IEnumerable<TipologiaCamera> GetAll()
        {
            var tipologie = new List<TipologiaCamera>();
            using (var connection = GetConnection())
            {
                 connection.Open();
                var command = GetCommand(GetAllQuery, connection);
                using (var reader =  command.ExecuteReader())
                {
                    while ( reader.Read())
                    {
                        tipologie.Add(new TipologiaCamera
                        {
                            Id = (int)reader["Id"],
                            Descrizione = reader["Descrizione"].ToString()
                        });
                    }
                }
            }
            return tipologie;
        }
    }
}
