using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BonGoo.Web.API.Data;

public class BonGooDbContextFactory : IDesignTimeDbContextFactory<BonGooDbContext>
{
    public BonGooDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BonGooDbContext>();
        
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found. Set ConnectionStrings__DefaultConnection environment variable.");
        
        optionsBuilder.UseNpgsql(connectionString);
        
        return new BonGooDbContext(optionsBuilder.Options);
    }
}