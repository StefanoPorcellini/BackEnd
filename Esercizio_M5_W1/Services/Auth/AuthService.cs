using Esercizio_M5_W1.Models;
using Esercizio_M5_W1.Services.V1;
using Microsoft.AspNetCore.Identity.Data;
using System.Data.SqlClient;

namespace Esercizio_M5_W1.Services.Auth
{
    public class AuthService : SqlServerServiceBase, IAuthService
    {
        private const string LOGIN_CMD = "SELECT Id FORM Users WHERE Username = @user AND Password = @psw";
        private const string ROLES_CMD = "Select r.Role FROM Users u JOIN Roles r ON u.Id_Role = r.Id WHERE u.Id = @user_id";

        public AuthService(IConfiguration config): base(config) { }

        public User Login(string username, string password)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = GetCommand(LOGIN_CMD);
                cmd.Parameters.Add(new SqlParameter("@user", username));
                cmd.Parameters.Add(new SqlParameter("@psw", password));
                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    var u = new User { Id = r.GetInt32(0), Username = username, Password = password };
                    r.Close();
                    using var roleCmd = GetCommand(ROLES_CMD);
                    roleCmd.Parameters.Add(new SqlParameter("@id", u.Id));
                    using var re = roleCmd.ExecuteReader();
                    while (re.Read())
                    {
                        u.Roles.Add(re.GetString(0));
                    }
                    return u;
                }
            }
            catch (Exception ex) { }
            return null;
        }        
    }
}
