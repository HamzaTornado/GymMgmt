using GymMgmt.Domain.Common;
using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using GymMgmt.Domain.Entities.Payments;
using GymMgmt.Domain.Entities.Plans;
using GymMgmt.Domain.Entities.Subsciptions;
using GymMgmt.Domain.Exceptions;

namespace GymMgmt.Domain.Entities.Members
{
    // Member.cs
    public class Member : AuditableEntity<MemberId>, IAggregateRoot
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string? Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; } = null!;
        public Address? Address { get; private set; }
        public bool HasPaidInsurance { get; private set; } = false;

        private readonly List<Subscription> _subscriptions = new();
        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();

        private readonly List<Payment> _payments = new();
        public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

        // EF Core
        private Member() { }

        internal Member(
            MemberId id,
            string firstName,
            string lastName,
            string? email,
            PhoneNumber phoneNumber,
            Address? address)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            HasPaidInsurance = false;
        }

        public static Member Create(
            string firstName,
            string lastName,
            string phoneNumber,
            string? email,
            Address? address) 
        {
            var member = new Member
            {
                Id = MemberId.New(),
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email?.Trim(),
                PhoneNumber = PhoneNumber.Create(phoneNumber),
                Address = address,
            };
            
            return member;
        }

        public string GetFullName() => $"{FirstName} {LastName}";
        public bool HasEmail() => !string.IsNullOrWhiteSpace(Email);

        // Update methods
        public void UpdateName(string firstName, string lastName)
        {
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public void UpdateEmail(string? email) =>
            Email = email?.Trim();

        public void UpdateAddress(Address? newAddress) =>
            Address = newAddress;

        public void UpdatePhoneNumber(string newPhoneNumber) =>
            PhoneNumber = PhoneNumber.Create(newPhoneNumber);
        public bool CanEnterGym(ClubSettings settings)
        {
            return Subscriptions.Any(s =>
                s.Status == SubscriptionStatus.Active &&
                !s.IsInGracePeriod(settings.SubscriptionGracePeriodInDays));
        }

        private void MarkInsuranceAsPaid(DateTime paymentDate)
        {
            // Check if they were ALREADY insured on the specific date they are trying to pay for.
            if (IsInsuredOn(paymentDate))
                throw new InsuranceFeeAlreadyPaidException(GetFullName());

            HasPaidInsurance = true;
        }
        public Subscription StartSubscription(
            MembershipPlan plan,
            InsuranceFee insuranceFee, 
            bool isInsuranceRequired, // Enforce insurance concept on the first subscription and adding later if the subscription expired by a year
            DateTime? startDate = null
            ) 
        {
            startDate ??= DateTime.Now;

            // Rule 1: no overlapping subscriptions (active during the same period)
            if (_subscriptions.Any(s => s.IsActiveOn(startDate.Value)))
            {
                throw new OverlappingSubscriptionException();
            }

            // Rule 2: insurance must be valid if required
            if (isInsuranceRequired && !IsInsuredOn(startDate.Value))
            {
                var check = IsInsuredOn(startDate.Value);
                throw new InsuranceFeeNotPaidException(Id.Value);
            }

            // Create the subscription
            var subscription = Subscription.Create(
                SubscriptionId.New(),
                startDate.Value,
                plan);

            _subscriptions.Add(subscription);

            return subscription;
        }

        /// <summary>
        /// Extends the current active subscription using a new plan.
        /// </summary>
        /// <returns>A tuple containing the NewPeriodStart and NewPeriodEnd of the extension.</returns>
        public (DateTime NewPeriodStart, DateTime NewPeriodEnd) ExtendCurrentSubscription(MembershipPlan extensionPlan)
        {
            var active = _subscriptions
                .FirstOrDefault(s => s.Status == SubscriptionStatus.Active)
                ?? throw new NoActiveSubscriptionException(Id.Value, "extend");

            // Rule: Cannot extend a subscription that is set to not renew.
            if (active.WillNotRenew)
            {
                throw new SubscriptionCannotBeExtendedException(active.Id.Value);
            }

            // Call the Extend method on the subscription itself
            // and pass its result (the new period) back up.
            return active.Extend(extensionPlan);
        }

        public void CancelCurrentSubscription()
        {
            var active = _subscriptions
                .FirstOrDefault(s => s.Status == SubscriptionStatus.Active)
                ?? throw new NoActiveSubscriptionException(Id.Value, "cancel");

            active.Revoke();
        }

        /// <summary>
        /// Finds the active subscription and "soft cancels" it.
        /// </summary>
        public void CancelCurrentSubscriptionAtPeriodEnd()
        {
            var active = _subscriptions
                .FirstOrDefault(s => s.Status == SubscriptionStatus.Active)
                ?? throw new NoActiveSubscriptionException(Id.Value, "cancel at end of period");

            active.CancelAtPeriodEnd();
        }
        /// <summary>
        /// Finds the active subscription and re-enables its renewal.
        /// </summary>
        public void EnableCurrentSubscriptionRenewal()
        {
            var active = _subscriptions
                .FirstOrDefault(s => s.Status == SubscriptionStatus.Active)
                ?? throw new NoActiveSubscriptionException(Id.Value, "enable renewal");

            active.EnableRenewal();
        }

        /// <summary>
        /// Finds the active subscription and "hard cancels" (bans) it.
        /// </summary>
        public void RevokeCurrentSubscription()
        {
            var active = _subscriptions
                .FirstOrDefault(s => s.Status == SubscriptionStatus.Active)
                ?? throw new NoActiveSubscriptionException(Id.Value, "revoke");

            active.Revoke();
        }

        public Payment RecordInsurancePayment(
            DateTime paymentDate,
            ClubSettings clubSettings,
            string? reference = null)
        {

            if (IsInsuredOn(paymentDate))
                throw new InsuranceFeeAlreadyPaidException(GetFullName());

            MarkInsuranceAsPaid(paymentDate);

            // --- NORMALIZE THE DATES ---

            // 1. Get the start of the day for the payment date
            var validFrom = paymentDate.Date; // e.g., 2025-11-11 00:00:00

            // 2. Calculate the end date. Add the days, then get the end of that day.
            var validUntil = validFrom
                .AddDays(clubSettings.InsuranceValidityInDays) // e.g., 2026-11-11 00:00:00
                .AddTicks(-1); // e.g., 2026-11-10 23:59:59.9999999
            var payment = Payment.CreateForInsurance(
                id: PaymentId.New(),
                memberId: Id,
                amount: clubSettings.CurrentInsuranceFee.Amount,
                paymentDate: paymentDate,
                validFrom: validFrom,
                validUntil: validUntil,
                reference: reference
                );
            _payments.Add(payment);
            return payment;
        }

        // Record subscription payment
        public Payment RecordSubscriptionPayment(
        Subscription subscription,
        DateTime paymentDate,
        DateTime periodStart, 
        DateTime periodEnd,   // <-- ADD THIS
        string? reference = null)
        {
            if (subscription.Status == SubscriptionStatus.Cancelled)
                throw new NoActiveSubscriptionException(Id.Value, "Pay For");

            var payment = Payment.CreateForSubscription(
                id: PaymentId.New(),
                memberId: Id,
                amount: subscription.Price, 
                paymentDate: paymentDate,
                subscriptionId: subscription.Id,
                periodStart: periodStart, 
                periodEnd: periodEnd,     
                reference: reference);

            _payments.Add(payment);
            return payment;
        }

        public bool IsInsuredOn(DateTime date)
        {
            return _payments
                .Where(p => p.Type == PaymentType.Insurance && p.IsSuccessful)
                .Any(p => p.PeriodStart <= date && p.PeriodEnd >= date);
        }

        public DateTime? GetCurrentInsuranceExpiry()
        {
            return _payments
                .Where(p => p.Type == PaymentType.Insurance && p.IsSuccessful)
                .Where(p => p.PeriodEnd >= DateTime.Today)
                .OrderByDescending(p => p.PeriodEnd)
                .Select(p => (DateTime?)p.PeriodEnd)
                .FirstOrDefault();
        }

        // Helpers
        public bool HasPaidInsuranceFee =>
            _payments.Any(p => p.Type == PaymentType.Insurance && p.IsSuccessful);

        public IEnumerable<Payment> GetSubscriptionPayments() =>
            _payments.Where(p => p.Type == PaymentType.Subscription && p.IsSuccessful);
    }
}
