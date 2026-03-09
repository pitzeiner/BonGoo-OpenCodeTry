using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BonGoo.Web.API.Data;

public class BonGooDbContextFactory : IDesignTimeDbContextFactory<BonGooDbContext>
{
    public BonGooDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BonGooDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=BonGoo;Username=postgres;Password=placeholder");
        
        return new BonGooDbContext(optionsBuilder.Options);
    }
}