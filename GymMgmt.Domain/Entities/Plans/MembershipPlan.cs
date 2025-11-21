using GymMgmt.Domain.Common;

namespace GymMgmt.Domain.Entities.Plans
{
    /// <summary>
    /// Represents a configurable gym membership package (e.g., 1-month, 3-month).
    /// Can be managed via admin panel.
    /// </summary>
    public class MembershipPlan : AuditableEntity<MembershipPlanId>, IAggregateRoot
    {
        public string Name { get; private set; } = string.Empty;
        public int DurationInMonths { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; } = true;

        // EF Core
        private MembershipPlan() { }

        private MembershipPlan(
            MembershipPlanId id,
            string name,
            int durationInMonths,
            decimal price)
        {
            Id = id;
            Name = name;
            DurationInMonths = durationInMonths;
            Price = price;
        }

        public static MembershipPlan Create(
                MembershipPlanId id,
                string name,
                int durationInMonths,
                decimal price)
        {

            return new MembershipPlan(id, name, durationInMonths, price);
        }

        public void Update(string name, int durationInMonths, decimal price)
        {

            Name = name;
            DurationInMonths = durationInMonths;
            Price = price;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public DateTime CalculateEndDate(DateTime startDate) =>
             startDate.AddMonths(DurationInMonths).AddDays(-1);

        public override string ToString() => $"{Name} ({DurationInMonths } Month(s))";
    }
}
