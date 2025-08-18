using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.Members
{
    public record MemberId(Guid Value)
    {
        public static MemberId FromValue(Guid value) => new(value);
        public static MemberId New() => FromValue(Guid.NewGuid());
    }
}
