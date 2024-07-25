using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Esercizio_Gestione_Albergo.ViewModels;
using Esercizio_Gestione_Albergo.Services.Sql;
using Microsoft.Extensions.Configuration;

namespace Esercizio_Gestione_Albergo.Services.DAO
{
    public class CameraDAO : SqlServerServiceBase, ICameraDAO
    {
        private const string GetAllQuery = "SELECT * FROM Camere";

        public CameraDAO(IConfiguration config) : base(config) { }

        public async Task<IEnumerable<CameraViewModel>> GetCamere()
        {
            var camere = new List<CameraViewModel>();

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                using (var command = GetCommand(GetAllQuery, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var camera = new CameraViewModel
                            {
                                Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                                Desc = reader.GetString(reader.GetOrdinal("Descrizione")),
                                Disponibile = reader.GetBoolean(reader.GetOrdinal("Disponibile")),
                                TipologiaId = reader.GetInt32(reader.GetOrdinal("TipologiaId"))
                            };

                            camere.Add(camera);
                        }
                    }
                }
            }

            return camere;
        }
    }
}
