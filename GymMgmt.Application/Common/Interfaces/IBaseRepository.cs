
namespace GymMgmt.Application.Common.Interfaces
{
    public interface IBaseRepository<TEntity, TId>
                where TEntity : class
                where TId : notnull
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TId id, CancellationToken cancellationToken = default);

        // Optional: Add if you need GetById
        Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
