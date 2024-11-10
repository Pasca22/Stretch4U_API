using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Stretch4U.Data;

namespace Stretch4U.Migrations;

internal class DesignTimeContextFactory : IDesignTimeDbContextFactory<Stretch4UContext>
{
    private const string _localSql = "server=localhost\\SQLEXPRESS; database=Stretch4U-Local; Integrated Security=True; Encrypt=False;";

    private static readonly string? _migrationAssemblyName = typeof(DesignTimeContextFactory).Assembly.GetName().Name;
    public Stretch4UContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Stretch4UContext>()
            .UseSqlServer(args.FirstOrDefault() ?? _localSql, 
                options =>options.MigrationsAssembly(_migrationAssemblyName));
   
        return new Stretch4UContext(optionsBuilder.Options);
    }
}