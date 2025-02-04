using Microsoft.AspNetCore.Mvc;
using PensionHackathonBackend.AdminPanel.Contract;
using PensionHackathonBackend.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionHackathonBackend.AdminPanel
{
    /* Класс контроллера администратора для реализации запросов по панели администратора */
    [Route("[controller]")]
    [ApiController]
    public class AdminController(IUserService userService)
        : ControllerBase
    {
        private readonly IUserService _userService = userService;

        /* Метод для проверки наличия пользователя по идентификатору */
        private async Task<bool> UserExists(int id)
        {
            var users = await _userService.GetAllUsers();

            return users.Any(user => user.Id == id);
        }

        /* Запрос на получение всех пользователей по поиску */
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetUsers(string searchString)
        {
            var users = await _userService.GetAllUsers();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = (List<Core.Models.User>)users.Where(set => set.Login.Contains(searchString)
                    || set.Role.Contains(searchString));
            }

            var response = users.ToList();

            return Ok(response);
        }

        /* Запрос на получение деталей по пользователю */
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                return BadRequest();
            }

            var users = await _userService.GetAllUsers();

            var response = users.Where(user => user.Id == id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /* Запрос на создание нового пользователя */
        [HttpPost]
        public async Task<ActionResult<int>> CreateUser(
            [FromBody] UserRequest request)
        {
            var (user, error) = Core.Models.User.Create(
                request.Login,
                request.Password,
                request.Role);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var userId = await _userService.CreateUser(user);

            return Ok(userId);
        }

        /* Запрос на обновление пользователя */
        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> UpdateUser(int id,
            [FromBody] UserRequest request)
        {
            return Ok(await _userService.UpdateUser(id, request.Login, request.Password, request.Role));
        }

        /* Запрос на сохранение пользователя после его изменения */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveUser(int id,
            [FromBody] UserRequest user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.UpdateUser(user.Id, user.Login,
                        user.Password, user.Role);

                    return RedirectToAction(nameof(GetUsers));

                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }

            return Ok(user);
        }

        /* Запрос на удаление пользователя */
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteUser(int id)
        {
            return Ok(await _userService.DeleteUser(id));
        }
    }
}
