namespace Esercizio_Gestione_Albergo.Models.Login
{
    public class LoginResponseModel
    {
        public int UserId { get; set; }

        public required string Username { get; set; }
        public required string Token { get; set; }
        public DateTime Expires { get; set; }

    }
}
