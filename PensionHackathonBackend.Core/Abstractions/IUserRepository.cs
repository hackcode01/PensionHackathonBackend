using PensionHackathonBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PensionHackathonBackend.Core.Abstractions
{
    /* Интерфейс пользователя для облегчения добавления новых методов */
    public interface IUserRepository
    {
        Task<List<User>> Get();
        Task<int> Create(User user);
        Task<User> GetByLogin(string login);
        Task<int> Update(int id, string login, string password, string role);
        Task<int> Delete(int id);
    }
}