using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.Sql;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public class ServizioAggiuntivoDAO : SqlServerServiceBase, IServizioAggiuntivoDAO
    {
        private const string GetAllQuery = "SELECT * FROM ServiziAggiuntivi";

        public ServizioAggiuntivoDAO(IConfiguration config) : base(config) { }


        public IEnumerable<ServizioAggiuntivo> GetAll()
        {
            var servizi = new List<ServizioAggiuntivo>();
            using (var connection = GetConnection())
            {
                 connection.Open();
                var command = GetCommand(GetAllQuery, connection);
                using (var reader =  command.ExecuteReader())
                {
                    while ( reader.Read())
                    {
                        servizi.Add(new ServizioAggiuntivo
                        {
                            ID = (int)reader["ID"],
                            Descrizione = reader["Descrizione"].ToString()
                        });
                    }
                }
            }
            return servizi;
        }
    }
}
