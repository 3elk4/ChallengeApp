using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ChallengeApp.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User>, IDbContext
    {
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Chore> Chores { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TEntity> GetSet<TEntity>() where TEntity : BaseAuditable
        {
            return this.Set<TEntity>();
        }
    }
}
