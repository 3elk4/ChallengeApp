using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallengeApp.Infrastructure.Configurations
{
    public class ChoreConfiguration : BaseEntityConfiguration<Chore>
    {
        public override void Configure(EntityTypeBuilder<Chore> builder)
        {
            base.Configure(builder);

            builder.Property(a => a.Title).HasMaxLength(250).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(1000).IsRequired();
            //builder.Property(a => a.Completed).
            builder.Property(a => a.Points).IsRequired();
            builder.Property(a => a.Difficulty).IsRequired();

            builder.HasOne(a => a.Challenge).WithMany(x => x.Chores).HasForeignKey(x => x.ChallengeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
