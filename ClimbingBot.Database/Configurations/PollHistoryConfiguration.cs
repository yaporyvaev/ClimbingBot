using ClimbingBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClimbingBot.Database.Configurations
{
    public class PollHistoryConfiguration : IEntityTypeConfiguration<PollHistory>
    {
        public void Configure(EntityTypeBuilder<PollHistory> builder)
        {
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.GroupId).IsRequired();
            builder.Property(x => x.MessageId).IsRequired();

            builder.HasIndex(i => i.GroupId);
        }
    }
}
