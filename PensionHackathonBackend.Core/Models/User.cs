using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PensionHackathonBackend.Core.Models
{
    /* Класс пользователя для дальнейшего представления сущности для базы данных */
    public class User
    {
        [Key, Required, NotNull]
        public int Id { get; set; }

        [Required, NotNull]
        public string Login { get; set; } = string.Empty;

        [Required, NotNull]
        public string Password { get; set; } = string.Empty;

        [Required, NotNull]
        public string Role { get; set; } = string.Empty;

        /* Закрытый конструктор для обеспечения инкапсуляции */
        private User( string login, string password, string role)
        {
            Login = login;
            Password = password;
            Role = role;
        }

        private User(int id, string login, string password, string role)
        {
            Id = id;
            Login = login;
            Password = password;
            Role = role;
        }

        /* Реализация паттерна 'Фабричный метод' в виде статического метода
         * по созданию объекта и возрата ошибки при наличии таковой
         */
        public static (User User, string Error) Create(string login,
            string password, string role)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(login))
            {
                error = "Login cannot be undefined or empty.";

            }

            if (string.IsNullOrEmpty(password))
            {
                error += "\nPassword cannot be undefined or empty.";
            }

            if (string.IsNullOrEmpty(role))
            {
                error += "\nRole cannot be undefined or empty.";
            }

            var user = new User( login, password, role);

            return (user, error);
        }
        
        public static (User User, string Error) Create(int id, string login,
            string password, string role)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(login))
            {
                error = "Login cannot be undefined or empty.";

            }

            if (string.IsNullOrEmpty(password))
            {
                error += "\nPassword cannot be undefined or empty.";
            }

            if (string.IsNullOrEmpty(role))
            {
                error += "\nRole cannot be undefined or empty.";
            }

            var user = new User( id, login, password, role);

            return (user, error);
        }
    }
}
