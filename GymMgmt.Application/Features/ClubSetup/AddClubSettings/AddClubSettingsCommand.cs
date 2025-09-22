using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.ClubSetup.GetClubSettings;


namespace GymMgmt.Application.Features.ClubSetup.AddClubSettings
{
    public sealed record AddClubSettingsCommand(
        decimal InssurranceFeeAmount,
        bool AreNewMembersAllowed,
        bool IsInsuranceFeeRequired,
        bool SubscriptionGracePeriodInDays,
        int InsuranceValidityInDays
        ) : ICommand<ReadClubSettingsDto>;
}
