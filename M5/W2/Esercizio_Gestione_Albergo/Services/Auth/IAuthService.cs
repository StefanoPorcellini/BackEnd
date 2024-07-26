using Esercizio_Gestione_Albergo.Models;

namespace Esercizio_Gestione_Albergo.Services.Auth
{
    public interface IAuthService
    {
         Task<Utente?> GetUtenteAsync(string username, string password);
    }
}
