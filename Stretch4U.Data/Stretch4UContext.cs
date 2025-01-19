using Microsoft.EntityFrameworkCore;
using Stretch4U.Data.Configurations;
using Stretch4U.Data.Models.Exercises;
using Stretch4U.Data.Models.ExercisesImages;
using Stretch4U.Data.Models.Messages;
using Stretch4U.Data.Models.Users;

namespace Stretch4U.Data;

public class Stretch4UContext : DbContext
{
    public Stretch4UContext() { }

    public Stretch4UContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<HamstringStretchImage> HamstringStretchImages { get; set; }
    public virtual DbSet<Exercise> Exercises { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration())
               .ApplyConfiguration(new MessageConfiguration())
               .ApplyConfiguration(new HamstringStretchImageConfiguration())
               .ApplyConfiguration(new ExerciseConfigurations());
    }
}
