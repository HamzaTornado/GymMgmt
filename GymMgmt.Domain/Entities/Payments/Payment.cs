using GymMgmt.Domain.Common;
using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Subsciptions;
using GymMgmt.Domain.Exceptions;


namespace GymMgmt.Domain.Entities.Payments
{
    // Payment.cs
    /// <summary>
    /// Represents a financial transaction made by a member (e.g., insurance, subscription).
    /// Immutable after creation — status can be updated.
    /// </summary>
    public class Payment : AuditableEntity<PaymentId>
    {
        public MemberId MemberId { get; private set; }
        public virtual Member Member { get; private set; } = null!;

        public PaymentType Type { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "MAD";
        public DateTime PaymentDate { get; private set; }

        // Optional: linked subscription (if applicable)
        public SubscriptionId? SubscriptionId { get; private set; }

        // Critical: Snapshot of the period this payment covers
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }

        public string? Reference { get; private set; } // Receipt, transaction ID
        public string? Notes { get; private set; }
        public PaymentStatus Status { get; private set; } = PaymentStatus.Paid;

        // EF Core
        private Payment() { }

        private Payment(
            PaymentId id,
            MemberId memberId,
            PaymentType type,
            decimal amount,
            DateTime paymentDate,
            DateTime periodStart,
            DateTime periodEnd,
            SubscriptionId? subscriptionId = null,
            string? reference = null,
            string? notes = null)
        {
            Id = id;
            MemberId = memberId;
            Type = type;
            Amount = amount;
            PaymentDate = paymentDate;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            SubscriptionId = subscriptionId;
            Reference = reference;
            Notes = notes;

            if (periodStart >= periodEnd)
                throw new SubscriptionPeriodInvalidException(periodStart,periodEnd);
        }

        /// <summary>
        /// Creates a payment for a subscription, with period snapshot.
        /// </summary>
        public static Payment CreateForSubscription(
            PaymentId id,
            MemberId memberId,
            decimal amount,
            DateTime paymentDate,
            SubscriptionId subscriptionId,
            DateTime periodStart,
            DateTime periodEnd,
            string? reference = null)
        {
            return new Payment(
                id: id,
                memberId: memberId,
                type: PaymentType.Subscription,
                amount: amount,
                paymentDate: paymentDate,
                periodStart: periodStart,
                periodEnd: periodEnd,
                subscriptionId: subscriptionId,
                reference: reference);
        }

        /// <summary>
        /// Creates a payment for insurance (no subscription, but still has a "validity period")
        /// </summary>
        public static Payment CreateForInsurance(
            PaymentId id,
            MemberId memberId,
            decimal amount,
            DateTime paymentDate,
            DateTime validFrom,
            DateTime validUntil,
            string? reference = null)
        {
            return new Payment(
                id: id,
                memberId: memberId,
                type: PaymentType.Insurance,
                amount: amount,
                paymentDate: paymentDate,
                periodStart: validFrom,
                periodEnd: validUntil,
                subscriptionId: null,
                reference: reference);
        }

        // Business logic
        public void Confirm(string? confirmedBy = null)
        {
            if (Status == PaymentStatus.Paid)
                throw new PaymentAlreadyConfirmedException(Id.Value);

            if (Status == PaymentStatus.Failed || Status == PaymentStatus.Refunded)
                throw new PaymentNotConfirmableException(Id.Value, Status);

            Status = PaymentStatus.Paid;
        }

        public void Refund(string? refundedBy = null, string? reason = null)
        {
            if (Status != PaymentStatus.Paid)
                throw new PaymentNotRefundableException(Id.Value, Status);

            Status = PaymentStatus.Refunded;
            Notes = $"Refunded: {reason}";
        }

        public bool IsSuccessful => Status == PaymentStatus.Paid;

        public override string ToString() =>
            $"{Type} - {Amount:C} for {PeriodStart:yyyy-MM-dd} → {PeriodEnd:yyyy-MM-dd} [{Status}]";
    }
}
