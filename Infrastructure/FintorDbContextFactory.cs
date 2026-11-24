using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

// Keeps EF tooling independent of API startup, JWT settings and hosted services.
public sealed class FintorDbContextFactory : IDesignTimeDbContextFactory<FintorDbContext>
{
    public FintorDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=Fintor;Trusted_Connection=True;TrustServerCertificate=True";
        return new FintorDbContext(new DbContextOptionsBuilder<FintorDbContext>()
            .UseSqlServer(connectionString).Options);
    }
}
