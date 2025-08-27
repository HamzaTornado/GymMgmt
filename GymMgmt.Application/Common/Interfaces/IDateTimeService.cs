using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Interfaces
{
    public interface IDateTimeService
    {
        DateTimeOffset Now { get; }
        DateTimeOffset UtcNow { get; }
    }
}
