using ClimbingBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClimbingBot.Database.Configurations
{
    public class TelegramGroupConfiguration : IEntityTypeConfiguration<TelegramGroup>
    {
        public void Configure(EntityTypeBuilder<TelegramGroup> builder)
        {
            builder.Property(x => x.GroupId).IsRequired();
            
            builder.HasIndex(i => i.GroupId);
        }
    }
}
