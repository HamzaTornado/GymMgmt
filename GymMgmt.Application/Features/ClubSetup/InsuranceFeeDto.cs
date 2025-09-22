using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.ClubSetup
{
    public sealed record InsuranceFeeDto(
        decimal Amount,
        string Currency,
        bool IsActive
        );
}
