using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Payments;
using GymMgmt.Domain.Entities.Subsciptions;
using MediatR;

namespace GymMgmt.Application.Features.Payments.CreatePaymentFor
{
    internal class CreatePaymentCommandHandler :IRequestHandler<CreatePaymentCommand,bool>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IClubSettingsRepository _clubSettingsRepository;


        public CreatePaymentCommandHandler(
            IMemberRepository memberRepository,
            IClubSettingsRepository clubSettingsRepository
            ) { 
            
            _memberRepository = memberRepository;
            _clubSettingsRepository = clubSettingsRepository;
        }

        public async Task<bool> Handle(CreatePaymentCommand request,CancellationToken cancellationToken)
        {
            var clubsettings = await _clubSettingsRepository.GetSingleOrDefaultAsync(cancellationToken);

            var member = await _memberRepository.FindByIdAsync(MemberId.FromValue(request.MemberId),cancellationToken);
            if (member == null) {
                throw new NotFoundException($"Member with ID {request.MemberId} Not Found");
            }

            if (clubsettings == null) {
                throw new NotFoundException("Club settings not Configured yet");
            }
            var paymentId = PaymentId.New();
            var dateTimenow = DateTime.Now;

            if (request.PaymentType == PaymentType.Insurance)
            {
                var insurancePayment = Payment.CreateForInsurance(
                    paymentId,
                    MemberId.FromValue(request.MemberId),
                    request.Amount,
                    dateTimenow,
                    request.PeriodStart,
                    request.PeriodEnd,
                    request.Refrence
                    );
               
                member.MarkInsuranceAsPaid();
            }
            else if (request.PaymentType == PaymentType.Subscription)
            {
                if (!request.SubscriptionId.HasValue)
                {
                    throw new NotFoundException("Subscription ID is required for subscription payments");
                }
                var subscriptionPayment = Payment.CreateForSubscription(
                        paymentId,
                        MemberId.FromValue(request.MemberId),
                        request.Amount,
                        dateTimenow,
                        SubscriptionId.FromValue(request.SubscriptionId.Value),
                        request.PeriodStart,
                        request.PeriodEnd,
                        request.Refrence

                );
            }


            return true;
        }
    }
}
