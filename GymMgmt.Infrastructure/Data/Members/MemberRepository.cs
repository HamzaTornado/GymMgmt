using GymMgmt.Domain.Entities.Members;

namespace GymMgmt.Infrastructure.Data.Members
{
    internal class MemberRepository(GymDbContext dbcontext) : BaseRepository<Member, MemberId>(dbcontext) ,IMemberRepository
    {

    }
}
