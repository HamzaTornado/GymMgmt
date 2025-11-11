using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.PayInsurrance
{
    internal class CreateInsurancePaymentValidator : AbstractValidator<CreateInsurancePaymentCommand>
    {
        public CreateInsurancePaymentValidator()
        {

            var now = DateTime.UtcNow;

            RuleFor(p => p.MemberId)
                .NotEmpty().WithMessage("Member ID is Required");

            RuleFor(x => x.PeriodStart)
                 .NotEmpty().WithMessage("Valid from date is required")
                 .LessThanOrEqualTo(now.AddDays(1))
                 .WithMessage("Valid from date cannot be more than 1 day in the future");

            RuleFor(x => x.Refrence)
                .Cascade(CascadeMode.Stop)
                .Must(r => string.IsNullOrWhiteSpace(r) || r.Length >= 3)
                .WithMessage("Reference must be at least 3 characters if provided")
                .Must(r => string.IsNullOrWhiteSpace(r) || r.All(char.IsLetterOrDigit))
                .WithMessage("Reference must be alphanumeric if provided");
        }
    }
}
