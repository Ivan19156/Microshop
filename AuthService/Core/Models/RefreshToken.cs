using Microsoft.AspNetCore.Identity;

namespace Models;
public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; } = default!;

    public ApplicationUser User { get; set; } = default!;
}
