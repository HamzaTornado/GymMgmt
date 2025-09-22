using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.ClubSetup.GetClubSettings
{
    public sealed record ReadClubSettingsDto(
        InsuranceFeeDto InsuranceFeeDto,
        bool AreNewMembersAllowed,
        bool IsInsuranceFeeRequired,
        int SubscriptionGracePeriodInDays,
        int InsuranceValidityInDays
        );
}
