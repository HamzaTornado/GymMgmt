using GymMgmt.Domain.Common;
using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Plans;
using GymMgmt.Domain.Exceptions;


namespace GymMgmt.Domain.Entities.Subsciptions
{
    public class Subscription : AuditableEntity<SubscriptionId>
    {
        public MemberId MemberId { get; private set; }
        public virtual Member Member { get; private set; } = null!;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string PlanName { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public SubscriptionStatus Status { get; private set; }
        public bool WillNotRenew { get; private set; } = false;

        private Subscription() { }

        private Subscription(
        SubscriptionId id,
        MemberId memberId, // 2. Add to Private Constructor
        DateTime startDate,
        DateTime endDate,
        MembershipPlan plan)
        {
            Id = id;
            MemberId = memberId; // 3. Set it
            StartDate = startDate;
            EndDate = endDate;
            PlanName = plan.Name;
            Price = plan.Price;
            Status = SubscriptionStatus.Active;
        }
        public static Subscription Create(
            SubscriptionId id,
            MemberId memberId,
            DateTime startDate,
            MembershipPlan plan)
        {
            // 1. Normalize start date: Nov 12, 2025 00:00:00
            var normalizedStartDate = startDate.Date;

            // 2. Calculate provisional end date using plan: Dec 11, 2025 00:00:00
            var provisionalEndDate = plan.CalculateEndDate(normalizedStartDate);

            // 3. Normalize end date to the end of that day: Dec 11, 2025 23:59:59...
            // We add 1 day and subtract 1 tick to get the very last moment of the day.
            var normalizedEndDate = provisionalEndDate.Date.AddDays(1).AddTicks(-1);

            return new Subscription(id,memberId, normalizedStartDate, normalizedEndDate, plan);
        }

        public (DateTime NewPeriodStart, DateTime NewPeriodEnd) Extend(MembershipPlan extensionPlan)
        {
            if (Status != SubscriptionStatus.Active)
                throw new NoActiveSubscriptionException(Id.Value, "extend");

            // 1. Calculate the start of the NEW period.
            // Since the old period ended at 23:59:59, we take the DATE component and add 1 day.
            // Example: Old End = Jan 14 23:59:59 -> Start = Jan 15 00:00:00
            var extensionStartDate = this.EndDate.Date.AddDays(1);

            // 2. Calculate provisional new end date
            var provisionalNewEndDate = extensionPlan.CalculateEndDate(extensionStartDate);

            // 3. Normalize the new end date to 23:59:59...
            var normalizedNewEndDate = provisionalNewEndDate.Date.AddDays(1).AddTicks(-1);

            // Set the new EndDate
            EndDate = normalizedNewEndDate;
            Price = extensionPlan.Price;
            PlanName = $"Extended: {extensionPlan.Name}";

            // Return the period we just calculated
            return (extensionStartDate, normalizedNewEndDate);
        }

        public void Activate()
        {
            if (Status == SubscriptionStatus.Active)
                throw new SubscriptionAlreadyActiveException(Id.Value);

            if (EndDate < DateTime.Today)
                throw new SubscriptionExpiredException(Id.Value);

            Status = SubscriptionStatus.Active;
        }
        public void Revoke(DateTime cancellationDate)
        {
            if (Status != SubscriptionStatus.Active)
                throw new NoActiveSubscriptionException(Id.Value, "revoke");

            // Safety 1: Don't extend it if it already ended in the past
            if (cancellationDate > EndDate) cancellationDate = EndDate;

            // Safety 2: Don't set end date before it even started
            if (cancellationDate < StartDate) cancellationDate = StartDate;

            // Apply the "Hard Cancel"
            EndDate = cancellationDate;
            Status = SubscriptionStatus.Revoked;
        }
        public void CancelAtPeriodEnd()
        {
            if (Status != SubscriptionStatus.Active)
                throw new NoActiveSubscriptionException(Id.Value, "cancel at end of period");

            WillNotRenew = true;
        }
        /// <summary>
        /// Re-enables renewal for a subscription that was "soft cancelled".
        /// </summary>
        public void EnableRenewal()
        {
            if (!WillNotRenew)
                throw new SubscriptionRenewalAlreadyEnabledException(Id.Value);

            WillNotRenew = false;
        }
        public bool IsActiveOn(DateTime date) =>
            Status == SubscriptionStatus.Active &&
            StartDate <= date &&
            EndDate >= date;
        public bool IsInGracePeriod(int graceDays)
        {
            return Status == SubscriptionStatus.Active &&
                   DateTime.Today > EndDate &&
                   DateTime.Today <= EndDate.AddDays(graceDays);
        }
        public override string ToString() =>
            $"{PlanName}: {StartDate:yyyy-MM-dd} → {EndDate:yyyy-MM-dd} [{Status}]";
    }
}

