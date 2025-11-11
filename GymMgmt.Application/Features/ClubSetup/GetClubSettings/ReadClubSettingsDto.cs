using GymMgmt.Domain.Entities.ClubSettingsConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.ClubSetup.GetClubSettings
{
    public sealed record ReadClubSettingsDto(
        Guid ClubSettingsId,
        InsuranceFeeDto InsuranceFee,
        bool AreNewMembersAllowed,
        bool IsInsuranceFeeRequired,
        int SubscriptionGracePeriodInDays,
        int InsuranceValidityInDays
        );
}
