using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SportSys.Database.Context;


public class SportSysDbContextFactory : IDesignTimeDbContextFactory<SportSysDbContext>
{
  public SportSysDbContext CreateDbContext(string[] args)
  {
    string settingsSuffixValue = string.Empty;

    //IConfigurationRoot configuration = new ConfigurationBuilder()
    //    //.SetBasePath(Directory.GetCurrentDirectory())
    //    .AddJsonFile(string.Format("appsettings{0}.json", string.IsNullOrEmpty(settingsSuffixValue) ? "" : "." + settingsSuffixValue))
    //    .Build();


    var builder = new DbContextOptionsBuilder<SportSysDbContext>();
    builder.UseSqlServer("Server=localhost\\SQL2022;Database=SportSys;Trusted_Connection=True;TrustServerCertificate=True;"); //, x => x.UseNetTopologySuite()
    return new SportSysDbContext(builder.Options);
  }
}

