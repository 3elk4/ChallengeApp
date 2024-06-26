using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IDbContext
    {
        DbSet<Challenge> Challenges { get; set; }
        DbSet<Chore> Chores { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<TEntity> GetSet<TEntity>() where TEntity : BaseAuditable;
    }
}
