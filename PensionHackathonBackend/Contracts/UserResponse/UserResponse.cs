using System;

namespace PensionHackathonBackend.Contracts.UserResponse;

public record UserResponse(
    Guid Id,
    string Login,
    string Role,
    string Password
);