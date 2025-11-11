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
        public int DurationInDays { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; } = true;

        // EF Core
        private MembershipPlan() { }

        private MembershipPlan(
            MembershipPlanId id,
            string name,
            int durationInDays,
            decimal price)
        {
            Id = id;
            Name = name;
            DurationInDays = durationInDays;
            Price = price;
        }

        public static MembershipPlan Create(
                MembershipPlanId id,
                string name,
                int durationInDays,
                decimal price)
        {

            return new MembershipPlan(id, name, durationInDays, price);
        }

        public void Update(string name, int durationInDays, decimal price)
        {

            Name = name;
            DurationInDays = durationInDays;
            Price = price;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public DateTime CalculateEndDate(DateTime startDate) =>
            startDate.AddDays(DurationInDays);

        public override string ToString() => $"{Name} ({DurationInDays} days)";
    }
}
