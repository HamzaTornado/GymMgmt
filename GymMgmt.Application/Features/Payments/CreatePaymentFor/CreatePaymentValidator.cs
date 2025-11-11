using FluentValidation;
using GymMgmt.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Payments.CreatePaymentFor
{
    internal class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentValidator() {

            var now = DateTime.UtcNow;

            RuleFor(p=>p.MemberId)
                .NotEmpty().WithMessage("Member ID is Required");

            RuleFor(p => p.Amount)
                .NotEmpty().WithMessage("The Amount is Required")
                .GreaterThan(0).WithMessage("The Amount must be valid");

            RuleFor(x => x.Currency)
                .Cascade(CascadeMode.Stop)
                .Must(r => string.IsNullOrWhiteSpace(r) || r.Length >= 3)
                .WithMessage("Currency must be at least 3 characters if provided");


            RuleFor(x => x.SubscriptionId)
                .NotEmpty()
                .When(x => x.PaymentType == PaymentType.Subscription) // Adjust based on your actual PaymentType values
                .WithMessage("Subscription ID is required for this payment type");

            RuleFor(x => x.PeriodStart)
                 .NotEmpty().WithMessage("Valid from date is required")
                 .LessThanOrEqualTo(now.AddDays(1))
                 .WithMessage("Valid from date cannot be more than 1 day in the future");

            RuleFor(x => x.PeriodEnd)
                 .NotEmpty().WithMessage("Valid Until date is required")
                 .LessThanOrEqualTo(now.AddDays(1))
                 .WithMessage("Valid from date cannot be more than 1 day in the future");

            RuleFor(x => x.Refrence)
                .Cascade(CascadeMode.Stop)
                .Must(r => string.IsNullOrWhiteSpace(r) || r.Length >= 3)
                .WithMessage("Reference must be at least 3 characters if provided")
                .Must(r => string.IsNullOrWhiteSpace(r) || r.All(char.IsLetterOrDigit))
                .WithMessage("Reference must be alphanumeric if provided");

            RuleFor(x => x.Note)
               .Cascade(CascadeMode.Stop)
               .Must(r => string.IsNullOrWhiteSpace(r) || r.Length >= 3)
               .WithMessage("Reference must be at least 3 characters if provided")
               .Must(r => string.IsNullOrWhiteSpace(r) || r.All(char.IsLetterOrDigit))
               .WithMessage("Note must be alphanumeric if provided");

            RuleFor(x => x.PaymentType)
                .IsInEnum().WithMessage("Invalid payment type");
        }
       
    }
}
