using FluentAssertions;
using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Plans;
using GymMgmt.Domain.Exceptions;
using GymMgmt.Domain.Tests.Builders;


namespace GymMgmt.Domain.Tests;


public class MemberTests
{
    [Fact]
    public void CreateMember_WithValidData_ShouldSucceed()
    {
        // Arrange
        var expectedFirstName = "Ali";
        var expectedLastName = "Benjelloun";
        var expectedPhone = "+212612345678";
        var expectedEmail = "ali@example.com";
        var expectedAddress = new Address("123 Rue Atlas", "Casablanca-Settat", "Casablanca", "20000");
        var expectedInsuranceFee = new InsuranceFee(default);

        var builder = new MemberBuilder()
            .WithFirstName(expectedFirstName)
            .WithLastName(expectedLastName)
            .WithPhoneNumber("0612345678")
            .WithEmail(expectedEmail)
            .WithAddress(expectedAddress)
            .IsInsuranceRequired(true);

        // Act
        var member = builder.Build();

        // Assert
        Assert.NotNull(member);
        Assert.Equal(expectedFirstName, member.FirstName);
        Assert.Equal(expectedLastName, member.LastName);
        Assert.Equal(expectedEmail, member.Email);
        Assert.Equal(expectedPhone, member.PhoneNumber.ToString()); 
        Assert.Equal(expectedAddress, member.Address);
        Assert.False(member.HasPaidInsurance);
        Assert.Empty(member.Subscriptions);
        Assert.Empty(member.Payments);
    }
    

    [Fact]
    public void CreateMember_WithInvalidPhoneNumber_ShouldThrowArgumentException()
    {
        // Arrange
        var firstName = "Ali";
        var lastName = "Benjelloun";
        var invalidPhoneNumber = "123"; // Not a valid Moroccan number
        var email = "ali@example.com";
        var address =new Address("123 Rue Atlas", "Casablanca-Settat", "Casablanca", "20000");
        var insuranceFee = new InsuranceFee(50m);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            Member.Create(firstName, lastName, invalidPhoneNumber, email, address));

        Assert.Contains("Numéro de téléphone marocain invalide", ex.Message);
    }

   
    
    [Fact]
    public void TryingToPay_Paid_InssuranceFee_ShouldThrowInvalidOperationException()
    {
        var insuranceFee = new InsuranceFee(default);
        var oneMonthPlan = MembershipPlan.Create(
            MembershipPlanId.New(),
            "1 Month",
            30,
            200m);

        var address = new Address("123 Rue Atlas", "Fès", "Fès-Meknes", "30000");

        var memberResult = Member.Create(
            "Hamza", "Zeroual", "0677740092", null, address);

        // Ensure member creation was successful before dereferencing  
        Assert.True(memberResult!=null);
        var member = memberResult;

        member.MarkInsuranceAsPaid();

        var ex = Assert.Throws<InsuranceFeeAlreadyPaidException>(() =>
        {
            member.MarkInsuranceAsPaid();
        });
        Assert.Equal("INSURANCE_FEE_ALREADY_PAID", ex.ErrorCode);
    }

    [Fact]
    public void ExtendSubscription_ForAnActiveOne_ShouldSucced()
    {
        var insuranceFee = new InsuranceFee(default);
        var oneMonthPlan = MembershipPlan.Create(
            MembershipPlanId.New(),
            "1 Month",
            30,
            200m);
        var builder = new MemberBuilder();
        var now = DateTime.Now;
        var member = builder.Build();

        member.MarkInsuranceAsPaid();
        var subscription = member.StartSubscription(oneMonthPlan, insuranceFee, isInsuranceRequired: true, now);
        subscription.Extend(oneMonthPlan);

        Assert.Equal(now.AddDays(60),subscription.EndDate);
        Assert.True(subscription.IsActiveOn(now.AddDays(60)));

    }
    [Fact]
    public void ExtendSubscription_For_NONActiveOne_ShouldFail()
    {
        var insuranceFee = new InsuranceFee(default);
        var oneMonthPlan = MembershipPlan.Create(
            MembershipPlanId.New(),
            "1 Month",
            30,
            200m);
        var builder = new MemberBuilder();
        var now = DateTime.Now;
        var member = builder.Build();

        member.MarkInsuranceAsPaid();
        var subscription = member.StartSubscription(oneMonthPlan, insuranceFee, isInsuranceRequired: true, now); 
        subscription.Cancel();

        var ex = Assert.Throws<NoActiveSubscriptionException>(() =>
        {
            subscription.Extend(oneMonthPlan);
        });

        Assert.Equal("NO_ACTIVE_SUBSCRIPTION", ex.ErrorCode);
    }
    [Fact]
    public void RecordInsurancePayment_WithValidData_ShoulSucced()
    {
        
        var insuranceFee = InsuranceFee.Default();
        var clubSettings = ClubSettings.Create(insuranceFee);
        var oneMonthPlan = MembershipPlan.Create(
            MembershipPlanId.New(),
            "1 Month",
            30,
            200m);
        var builder = new MemberBuilder();
        var now = DateTime.Now;
        var member = builder.Build();

        var payment = member.RecordInsurancePayment(now,clubSettings,"CASH-01");

        Assert.NotNull(payment);
        Assert.True(member.HasPaidInsurance);
        Assert.True(payment.IsSuccessful);
        Assert.Equal(100m,payment.Amount);
        Assert.Null(payment.SubscriptionId);
     }

    [Fact]
    public void RecordSubscriptionPayment_WithValidData_ShoulSucced()
    {

        var insuranceFee = InsuranceFee.Default();
        var clubSettings = ClubSettings.Create(insuranceFee);
        var oneMonthPlan = MembershipPlan.Create(
            MembershipPlanId.New(),
            "1 Month",
            30,
            200m);


        var builder = new MemberBuilder();
        var now = DateTime.Now;
        var member = builder.Build();

        var subscription = member.StartSubscription(oneMonthPlan, insuranceFee,clubSettings.IsInsuranceFeeRequired);

        var payment = member.RecordSubscriptionPayment(subscription, now, "CASH-Sub-01");

        Assert.NotNull(payment);
        Assert.True(payment.IsSuccessful);
        Assert.Equal(oneMonthPlan.Price, payment.Amount);
        Assert.Equal(subscription.Price, payment.Amount);
        Assert.NotNull(payment.SubscriptionId);
        Assert.Equal(subscription.StartDate,payment.PeriodStart);
        Assert.Equal(subscription.EndDate, payment.PeriodEnd);
        Assert.Equal(payment.PeriodStart.AddDays(oneMonthPlan.DurationInDays),payment.PeriodEnd);
    }


}
