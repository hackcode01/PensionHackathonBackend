using PensionHackathonBackend.Core.Models;

namespace PensionHackathonBackend.Infrastructure.Abstraction
{
    /* ИНтерфейс Jwt провайдера */
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}