using GymMgmt.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.Members
{
    public interface IMemberRepository : IBaseRepository<Member, MemberId>
    {

    }
}
