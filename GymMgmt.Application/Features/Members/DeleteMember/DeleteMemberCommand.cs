using GymMgmt.Application.Common.Interfaces;


namespace GymMgmt.Application.Features.Members.DeleteMember
{
    public record DeleteMemberCommand(
        Guid Id
    ) : ICommand<Guid>;
}
