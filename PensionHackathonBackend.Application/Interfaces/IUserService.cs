using PensionHackathonBackend.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PensionHackathonBackend.Application.Interfaces
{
    /* Интерфейс пользователя для облегчения добавления новых методов */
    public interface IUserService
    {
        Task<string> Login(string login, string password);

        Task Register(string login, string password, string role);

        Task<List<User>> GetAllUsers();

        Task<int> CreateUser(User user);

        Task<int> UpdateUser(int id, string login, string password, string role);

        Task<int> DeleteUser(int id);
    }
}
