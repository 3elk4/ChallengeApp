using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors
{
    class ChallengeInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUser _user;

        public ChallengeInterceptor(ICurrentUser user)
        {
            _user = user;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<Challenge>())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.Author == null)
                    {
                        entry.Entity.Author = _user.Id;
                    }
                }
            }
        }
    }
}
