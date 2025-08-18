namespace AuthService.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
   
    public string? AvatarUrl { get; set; }
    public string? Phone { get; set; }
}

