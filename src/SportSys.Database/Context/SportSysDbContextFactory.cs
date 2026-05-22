using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SportSys.Database.Context;

public class SportSysDbContextFactory : IDesignTimeDbContextFactory<SportSysDbContext>
{
    public SportSysDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=.\\SQL2022;Database=SportSys;Trusted_Connection=True;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<SportSysDbContext>();
        optionsBuilder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());

        return new SportSysDbContext(optionsBuilder.Options);
    }
}
