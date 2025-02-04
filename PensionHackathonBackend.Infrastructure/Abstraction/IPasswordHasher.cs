namespace PensionHackathonBackend.Infrastructure.Abstraction
{
    /* Интерфейс хэширования паролей */
    public interface IPasswordHasher
    {
        string Generate(string password);

        bool Verify(string hashedPassword, string providedPassword);
    }
}