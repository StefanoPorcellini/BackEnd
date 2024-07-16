using Esercizio_M5_W1.Models;

namespace Esercizio_M5_W1.Services.Auth
{
    public interface IAuthService
    {
        User Login(string username, string password);
    }
}
