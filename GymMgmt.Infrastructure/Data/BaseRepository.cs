using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Domain.Common;
using GymMgmt.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Data
{
    /// <summary>
    /// Generic EF Core repository implementation.
    /// Update operations are handled via Unit of Work and EF Core change tracking.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity ID type</typeparam>
    public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId>
    where TEntity : Entity<TId>, IAggregateRoot
    where TId : notnull
    {
        protected readonly GymDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new DbContextNullException();
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }
    }
}
