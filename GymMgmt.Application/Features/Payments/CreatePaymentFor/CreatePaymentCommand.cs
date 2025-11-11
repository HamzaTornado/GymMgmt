using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Domain.Common.Enums;


namespace GymMgmt.Application.Features.Payments.CreatePaymentFor
{
    public sealed record CreatePaymentCommand(
        Guid MemberId,
        decimal Amount,
        string Currency,
        Guid? SubscriptionId,
        PaymentType PaymentType, 
        DateTime PeriodStart,
        DateTime PeriodEnd,
        string? Refrence,
        string? Note
        ):ICommand<bool>;
}
