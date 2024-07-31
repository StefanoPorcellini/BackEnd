using Esercizio_Pizzeria_In_Forno.Models;

namespace Esercizio_Pizzeria_In_Forno.Service
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<User> LoginAsync(string username,  string password);

    }
}
