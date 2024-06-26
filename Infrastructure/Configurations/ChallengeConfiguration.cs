using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallengeApp.Infrastructure.Configurations
{
    public class ChallengeConfiguration : BaseEntityConfiguration<Challenge>
    {
        public override void Configure(EntityTypeBuilder<Challenge> builder)
        {
            base.Configure(builder);

            builder.Property(a => a.Title).HasMaxLength(250).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(1000).IsRequired();
            builder.Property(a => a.Type).IsRequired();
        }
    }
}
