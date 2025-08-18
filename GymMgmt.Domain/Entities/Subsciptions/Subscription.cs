using GymMgmt.Domain.Common;
using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Entities.Plans;
using GymMgmt.Domain.Exceptions;


namespace GymMgmt.Domain.Entities.Subsciptions
{
    public class Subscription : AuditableEntity<SubscriptionId>
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string PlanName { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public SubscriptionStatus Status { get; private set; } 

        private Subscription() { }

        private Subscription(
        SubscriptionId id,
        DateTime startDate,
        DateTime endDate,
        MembershipPlan plan)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            PlanName = plan.Name;
            Price = plan.Price;
            Status = SubscriptionStatus.Active;
        }

        public static Subscription Create(
        SubscriptionId id,
        DateTime startDate,
        MembershipPlan plan)
        {
            var endDate = plan.CalculateEndDate(startDate);

            return new Subscription(id, startDate, endDate, plan);
        }
        public void Activate()
        {
            if (Status == SubscriptionStatus.Active)
                throw new SubscriptionAlreadyActiveException(Id.Value);

            Status = SubscriptionStatus.Active;
        }
        public void Cancel()
        {
            if (Status != SubscriptionStatus.Active)
                throw new NoActiveSubscriptionException(Id.Value,"cancel");

            Status = SubscriptionStatus.Cancelled;
        }

        public void Extend(MembershipPlan extensionPlan)
        {
            if (Status != SubscriptionStatus.Active)
                throw new NoActiveSubscriptionException(Id.Value, "extend");

            EndDate = extensionPlan.CalculateEndDate(EndDate);
            Price = extensionPlan.Price;
            PlanName = $"Extended: {extensionPlan.Name}";
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

