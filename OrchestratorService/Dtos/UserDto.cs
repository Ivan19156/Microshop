namespace OrchestratorService.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public string Phone { get; set; } = default!;
    // public string? AvatarUrl { get; set; } // Якщо використовуєш
}
