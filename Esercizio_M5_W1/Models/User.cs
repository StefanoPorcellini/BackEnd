namespace Esercizio_M5_W1.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
