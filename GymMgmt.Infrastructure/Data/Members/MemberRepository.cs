using GymMgmt.Domain.Entities.Members;
using Microsoft.EntityFrameworkCore;

namespace GymMgmt.Infrastructure.Data.Members
{
    internal class MemberRepository(GymDbContext dbcontext) : BaseRepository<Member, MemberId>(dbcontext) ,IMemberRepository
    {
        public override async Task<Member?> FindByIdAsync(MemberId id, CancellationToken cancellationToken = default)
        {

            return await _dbSet
                .Include(m => m.Subscriptions)
                .Include(m => m.Payments)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
    }
}
