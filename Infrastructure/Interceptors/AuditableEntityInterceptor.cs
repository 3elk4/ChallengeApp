using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChallengeApp.Infrastructure.Interceptors
{
    class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUser _user;

        public AuditableEntityInterceptor(ICurrentUser user)
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

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.CreatedBy == null)
                    {
                        entry.Entity.CreatedBy = _user.Id;
                    }

                    entry.Entity.Created = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    if (entry.Entity.LastModifiedBy == null)
                    {
                        entry.Entity.LastModifiedBy = _user.Id;
                    }
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }
        }
    }
    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
