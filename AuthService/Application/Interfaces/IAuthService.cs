using AuthService.DTOs;
using System.Threading.Tasks;

public interface IAuthService
{
    Task<Guid> RegisterAsync(RegisterDto dto);
    Task<(string accessToken, string refreshToken)> LoginAsync(LoginDto dto);
    Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
    Task<UserDto?> GetUserByIdAsync(Guid userId);
}
