using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Account.CreateUser
{
    internal class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator() {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Le numéro de téléphone est requis.")
                .Matches(@"^(?:\+212|0)(5|6|7)\d{8}$")
                    .WithMessage("Numéro de téléphone invalide.")
                .MaximumLength(20)
                    .WithMessage("Le numéro de téléphone ne doit pas dépasser 20 caractères.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Must(p => p != null &&
                           p.Any(char.IsUpper) &&
                           p.Any(char.IsLower) &&
                           p.Any(char.IsDigit) &&
                           p.Any(ch => !char.IsLetterOrDigit(ch)))
                .WithMessage("Password must contain at least one uppercase, lowercase, digit and special character.");

            RuleFor(u => u.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(u => u.Password).WithMessage("Password and Confirm Password must match.")
                .When(u => !string.IsNullOrWhiteSpace(u.Password));

            RuleFor(u => u.Roles)
                .NotEmpty().WithMessage("At least one role is required.")
                .Must(r => r != null && r.All(role => !string.IsNullOrWhiteSpace(role)))
                .WithMessage("Role list contains empty or whitespace-only entries.");

        }
    }

}
