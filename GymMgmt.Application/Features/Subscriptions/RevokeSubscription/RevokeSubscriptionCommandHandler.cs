using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Domain.Entities.Members;
using MediatR;


namespace GymMgmt.Application.Features.Subscriptions.RevokeSubscription
{
    internal class RevokeSubscriptionCommandHandler : IRequestHandler<RevokeSubscriptionCommand,bool>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IDateTimeService _dateTimeService;

        public RevokeSubscriptionCommandHandler(
            IMemberRepository memberRepository,
            IDateTimeService dateTimeService)
        {
            _memberRepository = memberRepository;
            _dateTimeService = dateTimeService;
        }
        public async Task<bool> Handle(RevokeSubscriptionCommand request, CancellationToken ct)
        {
            var member = await _memberRepository.FindByIdAsync(MemberId.FromValue(request.MemberId),ct)
                    ?? throw new NotFoundException($"Member with ID {request.MemberId} Not Found");

            // Calls the destructive method with the current time
            member.RevokeCurrentSubscription(_dateTimeService.Now.DateTime);

            // Optional: Log the 'Reason' to an audit table or domain event

            return true;
        }
    }
}
