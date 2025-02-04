using PensionHackathonBackend.Application.Interfaces;
using PensionHackathonBackend.Core.Abstractions;
using PensionHackathonBackend.Core.Models;
using PensionHackathonBackend.Infrastructure.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PensionHackathonBackend.Application.Services
{
    /* Класс сервиса пользователя по реализации репозитория пользователя */
    public class UserService(IPasswordHasher passwordHasher,
        IUserRepository userRepository, IJwtProvider jwtProvider) : IUserService
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IUserRepository _usersRepository = userRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task Register(string login, string password, string role)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var (user, error) = User.Create(login, hashedPassword, role);

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            await _usersRepository.Create(user);
        }

        public async Task<string> Login(string login, string password)
        {
            var user = await _usersRepository.GetByLogin(login);

            var result = _passwordHasher.Verify(password, user.Password);

            if (result == false)
            {
                throw new Exception("Failed to login");
            }

            //var token = _jwtProvider.GenerateToken(user);

            return user.Role;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _usersRepository.Get();
        }

        public async Task<int> CreateUser(User user)
        {
            return await _usersRepository.Create(user);
        }

        public async Task<int> UpdateUser(int id, string login, string password, string role)
        {
            return await _usersRepository.Update(id, login, password, role);
        }

        public async Task<int> DeleteUser(int id)
        {
            return await _usersRepository.Delete(id);
        }
    }
}
