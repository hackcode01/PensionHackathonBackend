namespace PensionHackathonBackend.AdminPanel.Contract
{
    /* Запрос пользователя для панели администратора */
    public record UserRequest(
        int Id,
        string Login,
        string Role,
        string Password = ""
    );
}
