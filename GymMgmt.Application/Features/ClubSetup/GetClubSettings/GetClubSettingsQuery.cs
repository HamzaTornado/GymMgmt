using GymMgmt.Application.Common.Interfaces;

namespace GymMgmt.Application.Features.ClubSetup.GetClubSettings
{
    public sealed record GetClubSettingsQuery() : ICommand<ReadClubSettingsDto>;
}
