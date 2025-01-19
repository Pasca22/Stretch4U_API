using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stretch4U.Data.Models.Exercises;

namespace Stretch4U.Data.Configurations;

internal class ExerciseConfigurations : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(exercise => exercise.Id);
    }
}
