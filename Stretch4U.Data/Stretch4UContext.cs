using Microsoft.EntityFrameworkCore;
using Stretch4U.Data.Configurations;
using Stretch4U.Data.Models.Users;

namespace Stretch4U.Data;

public class Stretch4UContext : DbContext
{
    public Stretch4UContext() { }

    public Stretch4UContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
    }
}
