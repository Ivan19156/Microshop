using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    //public string AvatarUrl { get; set; } // Optional
    public DateTime? DateOfBirth { get; set; }
}
