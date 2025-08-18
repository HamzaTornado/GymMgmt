using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Tests.Clubinfos
{
    public class ClubSettingsTests
    {
        [Fact]
        public void Create_ClubSetting_WithvalidData()
        {
            // arrange assert 
            var insurancefee = new InsuranceFee(100m);


            var clubSetting = ClubSettings.Create(InsuranceFee.Default());


            Assert.NotNull(clubSetting);
            Assert.Equal(insurancefee.Amount, clubSetting.CurrentInsuranceFee.Amount);
            Assert.False(clubSetting.IsInsuranceFeeRequired);
            Assert.True(clubSetting.AreNewMembersAllowed);
            Assert.Equal(5, clubSetting.SubscriptionGracePeriodInDays);
            
        }

        [Fact]
        public void Update_InsuranceFee_ShouldSucced()
        {
            var insurancefee = new InsuranceFee(150m);

            var clubSetting = ClubSettings.Create(InsuranceFee.Default());

            clubSetting.UpdateInsuranceFee(insurancefee);


            Assert.NotNull(clubSetting);
            Assert.Equal(insurancefee.Amount, clubSetting.CurrentInsuranceFee.Amount);

        }

        [Fact]
        public void Update_GracePeriod_ShouldSucced()
        {
            var gracePreiodIdDays = 10;

            var clubSetting = ClubSettings.Create(InsuranceFee.Default());

            clubSetting.UpdateSubscriptionGracePeriod(gracePreiodIdDays);


            Assert.NotNull(clubSetting);
            Assert.Equal(gracePreiodIdDays, clubSetting.SubscriptionGracePeriodInDays);

        }

        [Fact]
        public void Update_InsuranceValidity_ShouldSucced()
        {
            var insuranceValidityIndays = 356*2;

            var clubSetting = ClubSettings.Create(InsuranceFee.Default());

            clubSetting.UpdateInsuranceValidity(insuranceValidityIndays);


            Assert.NotNull(clubSetting);
            Assert.Equal(insuranceValidityIndays, clubSetting.InsuranceValidityInDays);

        }


    }
}
