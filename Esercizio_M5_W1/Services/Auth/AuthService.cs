using Esercizio_M5_W1.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.Common;
using Esercizio_M5_W1.Services.V1;

namespace Esercizio_M5_W1.Services.Auth
{
    public class AuthService : SqlServerServiceBase, IAuthService
    {
        private const string LOGIN_CMD = "SELECT Id FROM Users WHERE Username = @user AND Password = @psw";
        private const string ROLES_CMD = "SELECT r.Role FROM Roles r JOIN Users u ON u.Id_Role = r.Id WHERE u.Id = @user_id";

        public AuthService(IConfiguration config) : base(config) { }

        public User Login(string username, string password)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = GetCommand(LOGIN_CMD, conn);
                cmd.Parameters.Add(new SqlParameter("@user", username));
                cmd.Parameters.Add(new SqlParameter("@psw", password));
                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    var u = new User { Id = r.GetInt32(0), Username = username, Password = password };
                    r.Close();
                    using var roleCmd = GetCommand(ROLES_CMD, conn);
                    roleCmd.Parameters.Add(new SqlParameter("@user_id", u.Id));
                    using var re = roleCmd.ExecuteReader();
                    while (re.Read())
                    {
                        u.Roles.Add(re.GetString(0));
                    }
                    return u;
                }
                else
                {
                    throw new UnauthorizedAccessException("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while trying to log in", ex);
            }
        }
    }
}
