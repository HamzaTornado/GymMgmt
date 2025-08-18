using GymMgmt.Domain.Common.Enums;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Plans;
using GymMgmt.Domain.Exceptions;
using System.Threading.Channels;


namespace GymMgmt.Domain.Tests.Subscriptions
{
    public class SubscriptionTest
    {
        [Fact]
        public void StartSubscription_WithoutPayingInsurance_ShouldFail()
        {
            var insuranceFee = new InsuranceFee(default);
            var oneMonthPlan = MembershipPlan.Create(
                MembershipPlanId.New(),
                "1 Month",
                30,
                200m);
            var now = DateTime.Now;
            var address = new Address("123 Rue Atlas", "Fès", "Fès-Meknes", "30000");

            var memberResult = Member.Create(
                "Hamza", "Zeroual", "0677740092", null, address);

            Assert.True(memberResult.IsSuccess);
            var mamber = memberResult.Value!;

            var ex = Assert.Throws<InsuranceFeeNotPaidException>(() =>
            {
                mamber.StartSubscription(oneMonthPlan, insuranceFee,true, now);
            });

            Assert.Contains("INSURANCE_FEE_NOT_PAID", ex.ErrorCode);
        }

        [Fact]
        public void StartSubscription_AfterPayingInsurance_ShouldSucceed()
        {
            // Arrange
            var insuranceFee = new InsuranceFee(50m);
            var oneMonthPlan = MembershipPlan.Create(
                MembershipPlanId.New(),
                "1 Month",
                30,
                200m);

            var address = new Address("123 Rue Atlas", "Fès", "Fès-Meknes", "30000");
            var memberResult = Member.Create(
                firstName: "Hamza",
                lastName: "Zeroual",
                phoneNumber: "0677740092",
                email: null,
                address: address);

            Assert.True(memberResult.IsSuccess);
            var member = memberResult.Value!;
            member.MarkInsuranceAsPaid();

            // Act
            var subscription = member.StartSubscription(oneMonthPlan, insuranceFee, true, DateTime.Now);

            // Assert
            Assert.NotNull(subscription);
            Assert.Equal(oneMonthPlan.Name, subscription.PlanName);
            Assert.Equal(oneMonthPlan.Price, subscription.Price);
            Assert.Equal(SubscriptionStatus.Active, subscription.Status);
            Assert.Contains(member.Subscriptions, s => s.Id == subscription.Id);
        }
        [Fact]
        public void StartSubscription_WhenAnotherIsActive_ShouldFail()
        {
            // Arrange
            var insuranceFee = new InsuranceFee(50m);
            var oneMonthPlan = MembershipPlan.Create(
                MembershipPlanId.New(),
                "1 Month",
                30,
                200m);

            var address = new Address("123 Rue Atlas", "Fès", "Fès-Meknes", "30000");
            var memberResult = Member.Create(
                "Hamza", "Zeroual", "0677740092", null, address);

            var member = memberResult.Value!;
            member.MarkInsuranceAsPaid();
            member.StartSubscription(oneMonthPlan, insuranceFee,isInsuranceRequired:true,DateTime.Now); // First subscription

            // Act & Assert
            var ex = Assert.Throws<OverlappingSubscriptionException>(() =>
                member.StartSubscription(oneMonthPlan, insuranceFee, isInsuranceRequired: true, DateTime.Now.AddDays(10)));

            Assert.Contains("OVERLAPPING_SUBSCRIPTION", ex.ErrorCode);
        }

        //[Fact]
        //public void CancelSubscription_ThatIsActive()
        //{

        //    // Arrange
        //    var insuranceFee = new InsuranceFee(50m);
        //    var oneMonthPlan = MembershipPlan.Create(
        //        MembershipPlanId.New(),
        //        "1 Month",
        //        30,
        //        200m);

        //    var address = new Address("123 Rue Atlas", "Fès", "Fès-Meknes", "30000");
        //    var member = Member.Create(
        //        firstName: "Hamza",
        //        lastName: "Zeroual",
        //        phoneNumber: "0677740092",
        //        email: null,
        //        address: address,
        //        insuranceFee: insuranceFee,
        //        isInsuranceRequired: true);

        //    member.MarkInsuranceAsPaid();

        //    // Act
        //    var subscription = member.StartSubscription(oneMonthPlan, DateTime.Today);

        //        subscription.Cancel();

        //    Assert.Equal(SubscriptionStatus.Cancelled,subscription.Status);
        //}

        //[Fact]
        //public void CancelSubscription_ThatIsNotActive(){

        //    // Arrange
        //    var insuranceFee = new InsuranceFee(50m);
        //    var oneMonthPlan = MembershipPlan.Create(
        //        MembershipPlanId.New(),
        //        "1 Month",
        //        30,
        //        200m);

        //    var address = new Address("123 Rue Atlas", "Fès", "Fès-Meknes", "30000");
        //    var member = Member.Create(
        //        firstName: "Hamza",
        //        lastName: "Zeroual",
        //        phoneNumber: "0677740092",
        //        email: null,
        //        address: address,
        //        insuranceFee: insuranceFee,
        //        isInsuranceRequired:true

        //        );

        //    member.MarkInsuranceAsPaid();

        //    // Act
        //    var subscription = member.StartSubscription(oneMonthPlan, DateTime.Today);
        //    subscription.Cancel();

        //    var ex = Assert.Throws<InvalidOperationException>(() =>
        //    {
        //        subscription.Cancel();
        //    });
        //    Assert.Contains("Cannot cancel subscription in status:", ex.Message);
        //}
    }
}
