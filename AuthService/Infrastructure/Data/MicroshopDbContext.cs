using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class ApplicationDbContext : IdentityDbContext
{ 
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ApplicationUser> applicationUsers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

