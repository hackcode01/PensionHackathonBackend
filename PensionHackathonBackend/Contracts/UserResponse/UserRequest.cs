namespace PensionHackathonBackend.Contracts.UserResponse;

public record UserRequest(
    string Login,
    string Password,
    string Role
);