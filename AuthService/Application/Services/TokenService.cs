using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Models;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateAccessToken(ApplicationUser user)
    {
        var claims = new[]
        {
           new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Fix: Convert Guid to string  
           new Claim(JwtRegisteredClaimNames.Email, user.Email!),
           new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
       };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            expires: DateTime.UtcNow.AddMinutes(15),
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(ApplicationUser user)
    {
        return new RefreshToken
        {
            Token = Guid.NewGuid().ToString("N"),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id // Fix: Directly assign the Guid value from ApplicationUser.Id  
        };
    }
}
