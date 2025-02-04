using System;

namespace PensionHackathonBackend.AdminPanel.Contract
{
    /* Ответ пользователя для панели администратора */
    public record UserResponse(
        Guid Id,
        string Login,
        string Role,
        string Password = ""
    );
}
