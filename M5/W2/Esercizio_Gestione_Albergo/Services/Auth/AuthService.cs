using Esercizio_Gestione_Albergo.DataAccess;
using Esercizio_Gestione_Albergo.Models;
using Esercizio_Gestione_Albergo.Services.Sql;
using System.Data.SqlClient;

namespace Esercizio_Gestione_Albergo.Services.Auth
{
    public class AuthService : SqlServerServiceBase, IAuthService
    {
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration config, ILogger<AuthService> logger) : base(config)
        {
            _logger = logger;
        }

        private const string SELECT_FOR_LOGIN = "SELECT UserId, Username, Password " +
                                                "FROM Utenti " +
                                                "WHERE Username = @username AND Password = @password";


        public async Task<Utente?> GetUtenteAsync(string username, string password)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = GetCommand(SELECT_FOR_LOGIN, conn);
                cmd.Parameters.Add(new SqlParameter("@username", username));
                cmd.Parameters.Add(new SqlParameter("@password", password));
                    
                var reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                    return new Utente
                    {
                        UserId = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2)
                    };
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eccezione nel login di un autore");
                throw;
            }
        }
    }
}
