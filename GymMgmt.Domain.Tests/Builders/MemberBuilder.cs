using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Tests.Builders
{
    public class MemberBuilder
    {
        private string _firstName = "Ali";
        private string _lastName = "Benjelloun";
        private string _phoneNumber = "0612345678";
        private string? _email = "ali@example.com";
        private Address? _address = new("123 Rue Atlas", "Casablanca-Settat", "Casablanca", "20000");
        private InsuranceFee _insuranceFee = new InsuranceFee(default);
        private bool _isInsuranceRequired = true;
        private MembershipPlan _OneMonthPlan = MembershipPlan.Create(
            MembershipPlanId.New(),
            "1 Month",
            30,
            200m);

        public MemberBuilder WithFirstName(string firstName) { _firstName = firstName; return this; }
        public MemberBuilder WithLastName(string lastname) { _lastName = lastname; return this; }
        public MemberBuilder WithPhoneNumber(string phoneNumber) { _phoneNumber = phoneNumber; return this; }
        public MemberBuilder WithoutEmail() { _email = null; return this; }
        public MemberBuilder WithAddress(Address address) { _address = address; return this; }
        public MemberBuilder WithEmail(string address) { _email = address; return this; }
        public MemberBuilder WithInsuranceFee(InsuranceFee insuranceFee) { _insuranceFee = insuranceFee; return this; }
        public MemberBuilder WithoutAddressl() { _address = null; return this; }

        public MemberBuilder IsInsuranceRequired(bool isInsuranceRequired) { _isInsuranceRequired = isInsuranceRequired; return this; }

        public Member Build()
        {
            var result = Member.Create(_firstName, _lastName, _phoneNumber, _email, _address);
            if (result==null)
            {
                throw new InvalidOperationException("Failed to create Member: ");
            }
            return result;


        }
    }
}
