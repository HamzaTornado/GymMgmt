using GymMgmt.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Tests.InsuranceFeeTests
{
    public class InsuranceFeeTest
    {
        [Fact]
        public void Create_InsuranceFee_WithvalidData()
        {
            var insuranceFee = new InsuranceFee(150m,true);

            Assert.NotNull(insuranceFee);

            Assert.True(insuranceFee.IsActive);
            Assert.Equal(150m,insuranceFee.Amount);
            Assert.Equal("MAD", insuranceFee.Currency);   
        }

        [Fact]
        public void Checking_InfuranceFee_theDefaulData()
        {
            var defaultInsuranceFee = InsuranceFee.Default();

            Assert.NotNull(defaultInsuranceFee);
            Assert.True(defaultInsuranceFee.IsActive);
            Assert.Equal("MAD", defaultInsuranceFee.Currency);
            Assert.Equal(100m, defaultInsuranceFee.Amount);

        }
    }
}
