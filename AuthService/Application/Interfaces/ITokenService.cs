using Microsoft.AspNetCore.Identity;
using Models;

public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user);
    RefreshToken GenerateRefreshToken(ApplicationUser user);
}
