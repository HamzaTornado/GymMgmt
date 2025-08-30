using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Domain.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Interceptors
{
    internal class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly TimeProvider _dateTime;

        public AuditableEntityInterceptor(
            ICurrentUserService currentUserService,
            TimeProvider dateTime)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
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

            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    var utcNow = _dateTime.GetUtcNow();
                    var userName = _currentUserService.UserName ?? "System";
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.SetCreationInfo(utcNow, userName);
                    }
                    else
                    {
                        entry.Entity.SetModificationInfo(utcNow, userName);
                    }
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
