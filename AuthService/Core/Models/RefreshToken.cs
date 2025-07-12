using Microsoft.AspNetCore.Identity;

namespace Models;
public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public string UserId { get; set; } = default!;

    public IdentityUser User { get; set; } = default!;
}
