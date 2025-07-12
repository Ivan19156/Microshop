using Microsoft.AspNetCore.Identity;
using Models;

public interface ITokenService
{
    string GenerateAccessToken(IdentityUser user);
    RefreshToken GenerateRefreshToken(IdentityUser user);
}
