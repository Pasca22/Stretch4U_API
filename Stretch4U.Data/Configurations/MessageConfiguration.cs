using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stretch4U.Data.Models.Messages;

namespace Stretch4U.Data.Configurations;

internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(message => message.Id);
        builder.HasIndex(message => message.UserId);

        builder.HasOne(message => message.User)
              .WithMany(user => user.Messages)
              .HasForeignKey(message => message.UserId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}
