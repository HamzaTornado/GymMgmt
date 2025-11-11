using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.ClubSetup.GetClubSettings;


namespace GymMgmt.Application.Features.ClubSetup.AddClubSettings
{
    public sealed record CreateClubSettingsCommand(
        decimal InsurranceFeeAmount,
        bool AreNewMembersAllowed = true,
        bool IsInsuranceFeeRequired=false,
        int SubscriptionGracePeriodInDays = 0,
        int InsuranceValidityInDays = 0,
        bool ParDefaut =false
        ) : ICommand<ReadClubSettingsDto>;
}
