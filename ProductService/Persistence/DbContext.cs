using Entities;
using Microsoft.EntityFrameworkCore;
namespace ProductDbContext;

// public class AppDbContext : DbContext
// {
//     public DbSet<Product> Products { get; set; }

//     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
//     }
// }

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}