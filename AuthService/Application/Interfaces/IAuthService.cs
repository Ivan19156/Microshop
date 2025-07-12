using System.Threading.Tasks;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<(string accessToken, string refreshToken)> LoginAsync(LoginDto dto);
    Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
}
