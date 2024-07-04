using Esercizio_M3_W3_D4.Models;


using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.Common;

namespace Esercizio_M3_W3_D4.Serices
{
    public class ImpiegatoService : SqlServerServiceBase, I_Impiegato
    {
        public ImpiegatoService(IConfiguration config) : base(config) { } 

        public Impiegato Create(DbDataReader reader)
        {
            return new Impiegato
            {
                IDImpiegato = reader.GetInt32(0),
                Cognome = reader.GetString(1),
                Nome = reader.GetString(2),
                CodiceFiscale = reader.GetString(3),
                Eta = reader.GetInt32(4),
                RedditoMensile = reader.GetDecimal(5),
                DetrazioneFiscale = reader.GetBoolean(6)
            };
        }

        public IEnumerable<Impiegato> GetAll() {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = GetCommand("SELECT IDImpiegato,Cognome, Nome, CodiceFiscale, Eta, RedditoMensile, DetrazioneFiscale FROM Impiegato");
            using var reader = cmd.ExecuteReader();
            var list = new List<Impiegato>();
            while (reader.Read())
                list.Add(Create(reader));
            conn.Close();
            return list;
        }
    }
}