using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Common
{
    public interface IBaseRepository<TEntity, TId> where TEntity : Entity<TId>, IAggregateRoot
    {
        Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Delete(TEntity entity);
    }
}
