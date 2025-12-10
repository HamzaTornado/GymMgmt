using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Payments;
using MediatR;


namespace GymMgmt.Application.Features.Members.PayInsurrance
{
    internal class CreateInsurancePaymentCommandHandler :IRequestHandler<CreateInsurancePaymentCommand,bool>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IClubSettingsRepository _clubSettingsRepository;


        public CreateInsurancePaymentCommandHandler(
            IMemberRepository memberRepository,
            IClubSettingsRepository clubSettingsRepository
            )
        {

            _memberRepository = memberRepository;
            _clubSettingsRepository = clubSettingsRepository;
        }

        public async Task<bool> Handle( CreateInsurancePaymentCommand request,CancellationToken cancellationToken)
        {

            var clubsettings = await _clubSettingsRepository.GetSingleOrDefaultAsync(cancellationToken);

            var member = await _memberRepository.FindByIdAsync(MemberId.FromValue(request.MemberId), cancellationToken);
            if (member == null)
            {
                throw new NotFoundException($"Member with ID {request.MemberId} Not Found");
            }

            if (clubsettings == null)
            {
                throw new NotFoundException("Club settings not Configured yet");
            }

            var paymentId = PaymentId.New();
            var dateTimenow = DateTime.Now;

            var payment =member.RecordInsurancePayment(dateTimenow, clubsettings, request.Refrence);



            return payment != null;
        }
    }
}
