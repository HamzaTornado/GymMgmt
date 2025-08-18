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

        private Member(
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

        public static Result<Member> Create(
            string firstName,
            string lastName,
            string phoneNumber,
            string? email,
            Address? address) 
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result<Member>.Failure(ValidationError.Required(nameof(firstName)));
            if (string.IsNullOrWhiteSpace(lastName))
                return Result<Member>.Failure(ValidationError.Required(nameof(lastName)));
            

            var member = new Member(
                MemberId.New(),
                firstName.Trim(),
                lastName.Trim(),
                email?.Trim(),
                PhoneNumber.Create(phoneNumber),
                address
            );

            // raise: MemberCreatedDomainEvent(member.Id, insuranceFee)
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

        public void MarkInsuranceAsPaid()
        {
            if (HasPaidInsurance)
                throw new InsuranceFeeAlreadyPaidException(GetFullName());

            HasPaidInsurance = true;
            //AddDomainEvent(new InsuranceFeePaid(Id));
        }

        public Subscription StartSubscription(MembershipPlan plan, 
            InsuranceFee insuranceFee, // ← from ClubSettings.CurrentInsuranceFee
            bool isInsuranceRequired, // Enforce insurance concept on the first subscription and adding later if the subscription expired by a year
            DateTime? startDate = null
            ) 
        {

            startDate ??= DateTime.Now;

            // Business rule 1: No overlapping active subscriptions
            if (_subscriptions.Any(s => s.IsActiveOn(startDate.Value)))
            {
                throw new OverlappingSubscriptionException();
            }

            if (isInsuranceRequired && !HasPaidInsurance)
                throw new InsuranceFeeNotPaidException(Id.Value);

            // Allow renewal: only block if there's an *active* subscription
            if (_subscriptions.Any(s => s.IsActiveOn(startDate.Value)))
            {
                throw new ActiveSubscriptionExistsException(Id.Value);
            }

            var subscription = Subscription.Create(
                SubscriptionId.New(),
                startDate.Value,
                plan);

            _subscriptions.Add(subscription);
            return subscription;
        }

        public void ExtendCurrentSubscription(MembershipPlan extensionPlan)
        {
            var active = _subscriptions
                .FirstOrDefault(s => s.Status == SubscriptionStatus.Active)
                ?? throw new NoActiveSubscriptionException(Id.Value,"extend");

            active.Extend(extensionPlan);
        }
        public void CancelCurrentSubscription()
        {
            var active = _subscriptions
                .FirstOrDefault(s => s.Status == SubscriptionStatus.Active)
                ?? throw new NoActiveSubscriptionException(Id.Value, "cancel");

            active.Cancel();
        }

        

        // Add payment (called when insurance is paid)
        public Payment RecordInsurancePayment(
            decimal amount,
            DateTime paymentDate,
            ClubSettings clubSettings,
            string? reference = null)
        {

            if (IsInsuredOn(paymentDate))
                throw new InsuranceAlreadyActiveException(Id.Value,paymentDate);

            var validUntil = paymentDate.AddDays(clubSettings.InsuranceValidityInDays);
            var payment = Payment.ForInsurance(
                id: PaymentId.New(),
                memberId: Id,
                amount: amount,
                paymentDate: paymentDate,
                validFrom: paymentDate,
                validUntil: validUntil,
                reference: reference
                );

            _payments.Add(payment);
            MarkInsuranceAsPaid(); // Already updates flag

            return payment;
        }
        
        // Record subscription payment
        public Payment RecordSubscriptionPayment(
            Subscription subscription,
            DateTime paymentDate,
            string? reference = null)
        {
            if (subscription.Status == SubscriptionStatus.Cancelled)
                throw new NoActiveSubscriptionException(Id.Value,"Pay For");

            var payment = Payment.ForSubscription(
                id: PaymentId.New(),
                memberId: Id,
                amount: subscription.Price,
                paymentDate: paymentDate,
                subscriptionId: subscription.Id,
                periodStart:subscription.StartDate,
                periodEnd:subscription.EndDate,
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
