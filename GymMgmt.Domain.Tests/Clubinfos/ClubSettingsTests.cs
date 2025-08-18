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
    }
}
