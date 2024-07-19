using Esercizio_M5_W1.Models;
using System.Data.SqlClient;

namespace Esercizio_M5_W1.Services.V1
{
    public class SpedizioneService : SqlServerServiceBase, ISpedizioneService
    {
        public SpedizioneService(IConfiguration config) : base (config) { }

        public IEnumerable<Spedizione> GetShipById(int userId)
        {
            var query = "SELECT s.* FROM Spedizioni s " +
                        "INNER JOIN Clienti c ON s.Id_Cliente = c.Id " +
                        "INNER JOIN Users u ON c.Id_user = u.Id " +
                        "WHERE u.Id = @userId";
            var spedizioni = new List<Spedizione>();

            using var conn = GetConnection();
            conn.Open();
            using var cmd = GetCommand(query, conn);
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var spedizione = new Spedizione
                {
                    Id = reader.GetInt32(0),
                    Id_Cliente = reader.GetInt32(1),
                    N_Identificativo = reader.GetString(2),
                    DataSpedizione = reader.GetDateTime(3),
                    Peso = reader.GetDouble(4),
                    CittaDestinaratia = reader.GetString(5),
                    IndirizzoDestinazione = reader.GetString(6),
                    NominativoDestinazione = reader.GetString(7),
                    Costo = reader.GetDecimal(8),
                    DataConsegnaPrevista = reader.GetDateTime(9)
                };
                spedizioni.Add(spedizione);
            }

            return spedizioni;
        }
                public void CreaSpedizione(Spedizione spedizione)
                {
                    var query = "INSERT INTO Spedizioni (Id_Cliente, NumeroIdentificativo, DataSpedizione, Peso, CittaDestinataria, IndirizzoDestinatario, " +
                                "NominativoDestinazione, Costo, DataConsegnaPrevista) VALUES (@Id_Cliente, @N_Identificativo, @DataSpedizione, @Peso, " +
                                "@CittaDestinataria, @IndirizzoDestinazione, @NominativoDestinazione, @Costo, @DataConsegnaPrevista)";
                    using var conn = GetConnection();
                    conn.Open();

                    using var cmd = GetCommand(query, conn);
                    cmd.Parameters.Add(new SqlParameter("@Id_Cliente", spedizione.Id_Cliente));
                    cmd.Parameters.Add(new SqlParameter("@NumeroIdentificativo", spedizione.N_Identificativo));
                    cmd.Parameters.Add(new SqlParameter("@DataSpedizione", spedizione.DataSpedizione));
                    cmd.Parameters.Add(new SqlParameter("@Peso", spedizione.Peso));
                    cmd.Parameters.Add(new SqlParameter("@CittaDestinataria", spedizione.CittaDestinaratia));
                    cmd.Parameters.Add(new SqlParameter("@IndirizzoDestinatario", spedizione.IndirizzoDestinazione));
                    cmd.Parameters.Add(new SqlParameter("@NominativoDestinazione", spedizione.NominativoDestinazione));
                    cmd.Parameters.Add(new SqlParameter("@Costo", spedizione.Costo));
                    cmd.Parameters.Add(new SqlParameter("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista));

                    cmd.ExecuteNonQuery();
                }
    }
}
