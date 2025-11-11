using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.ClubSetup.GetClubSettings;
using GymMgmt.Domain.Entities.ClubSettingsConfig;


namespace GymMgmt.Application.Features.ClubSetup.UpdateClubSettings
{
    public sealed record UpdateClubSettingsCommand(
        ClubSettingsId ClubSettingsId,
        decimal InsurranceFeeAmount,
        bool AreNewMembersAllowed,
        bool IsInsuranceFeeRequired,
        int SubscriptionGracePeriodInDays,
        int InsuranceValidityInDays
        ) : ICommand<ReadClubSettingsDto>;
}
