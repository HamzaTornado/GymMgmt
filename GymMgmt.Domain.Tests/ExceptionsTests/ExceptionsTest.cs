using GymMgmt.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Tests.ExceptionsTests
{
    public class ExceptionsTest
    {
        [Fact]
        public void ActiveSubscriptionExistsException_HasCorrectMessage()
        {
            var ex = new ActiveSubscriptionExistsException(Guid.NewGuid());
            Assert.Equal("ACTIVE_SUBSCRIPTION_EXISTS", ex.ErrorCode);
            Assert.Contains("has an active one", ex.Message);
        }

        [Fact]
        public void InsuranceFeeAlreadyPaidException_HasCorrectMessage()
        {
            var ex = new InsuranceFeeAlreadyPaidException("Alice");
            Assert.Equal("INSURANCE_FEE_ALREADY_PAID", ex.ErrorCode);
            Assert.Contains("Alice", ex.Message);
        }

        [Fact]
        public void InsuranceFeeNotPaidException_HasCorrectMessage()
        {
            var ex = new InsuranceFeeNotPaidException(Guid.NewGuid());
            Assert.Equal("INSURANCE_FEE_NOT_PAID", ex.ErrorCode);
            Assert.Contains("insurance fee", ex.Message);
        }

        [Fact]
        public void InsuranceFeeRequiredException_HasCorrectMessage()
        {
            var ex = new InsuranceFeeRequiredException();
            Assert.Equal("INSURANCE_FEE_REQUIRED", ex.ErrorCode);
            Assert.Contains("Initial insurance fee is required", ex.Message);
        }

        [Fact]
        public void OverlappingSubscriptionException_HasCorrectMessage()
        {
            var ex = new OverlappingSubscriptionException();
            Assert.Equal("OVERLAPPING_SUBSCRIPTION", ex.ErrorCode);
            Assert.Contains("overlapping active subscriptions", ex.Message);
        }
    }
}
