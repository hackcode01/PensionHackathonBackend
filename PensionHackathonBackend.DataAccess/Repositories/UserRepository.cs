using Microsoft.EntityFrameworkCore;
using PensionHackathonBackend.Core.Abstractions;
using PensionHackathonBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionHackathonBackend.DataAccess.Repositories
{
    /* Репозиторий пользователя для дальнейшей реализации CRUD запросов */
    public class UserRepository(PensionHackathonDbContext context) : IUserRepository
    {
        private readonly PensionHackathonDbContext _context = context;

        /* Получение пользователя по его логину */
        public async Task<User> GetByLogin(string login)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Login == login) ??
                throw new Exception();

            return userEntity;
        }

        /* Получение пользователей */
        public async Task<List<User>> Get()
        {
            var userEntities = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            var users = userEntities
                .Select(user => User
                    .Create(user.Login, user.Password, user.Role).User)
                .ToList();

            return users;
        }

        /* Создание нового пользователя */
        public async Task<int> Create(User user)
        {
            var userEntity = User.Create(user.Login, user.Password, user.Role);

            await _context.Users.AddAsync(userEntity.User);
            await _context.SaveChangesAsync();

            return userEntity.User.Id;
        }

        /* Обновление пользователя */
        public async Task<int> Update(int id, string login, string password, string role)
        {
            await _context.Users
                .Where(user => user.Id == id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(user => user.Login, user => login)
                    .SetProperty(user => user.Password, user => password)
                    .SetProperty(user => user.Role, user => user.Role));

            return id;
        }

        /* Удаление пользователя */
        public async Task<int> Delete(int id)
        {
            await _context.Users
                .Where(user => user.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}
