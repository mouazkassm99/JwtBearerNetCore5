using JwtToken.Data;

namespace JwtToken.Services
{
    public interface ITokenService
    {
        string BuildToken(UserReadDto user);
        bool ValidateToken(string token);
    }
}