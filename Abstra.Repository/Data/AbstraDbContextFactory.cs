using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Abstra.Repository.Data;

public class AbstraDbContextFactory : IDesignTimeDbContextFactory<AbstraDbContext>
{
    public AbstraDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AbstraDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AbstraDb;Trusted_Connection=true;TrustServerCertificate=true");

        return new AbstraDbContext(optionsBuilder.Options);
    }
}
