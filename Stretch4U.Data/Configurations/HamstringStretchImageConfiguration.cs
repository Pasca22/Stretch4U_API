using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stretch4U.Data.Models.ExercisesImages;

namespace Stretch4U.Data.Configurations;

internal class HamstringStretchImageConfiguration : IEntityTypeConfiguration<HamstringStretchImage>
{
    public void Configure(EntityTypeBuilder<HamstringStretchImage> builder)
    {
        builder.HasKey(message => message.Id);
        builder.HasIndex(message => message.UserId);

        builder.HasOne(message => message.User)
               .WithMany(user => user.HamstringStretchImages)
               .HasForeignKey(message => message.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
