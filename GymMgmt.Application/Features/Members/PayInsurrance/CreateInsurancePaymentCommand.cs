using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.PayInsurrance
{
    public sealed record CreateInsurancePaymentCommand(
         Guid MemberId,
        DateTime PeriodStart,
        string? Refrence
        ) : ICommand<bool>;
        
}
