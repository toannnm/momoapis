using Domain.Entities;

namespace Application.Interfaces.IExtensionServices
{
    public interface IJwtService
    {
        string Hash(string password, string salt);
        string Salt();
        bool Verify(string password, string stringVerify);
        string GenerateToken(User user);
    }
}