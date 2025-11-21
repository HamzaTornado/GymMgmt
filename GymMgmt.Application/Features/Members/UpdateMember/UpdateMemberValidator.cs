using AuthSystem.Application.Features.Memebers;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.UpdateMember
{
    internal class UpdateMemberValidator : AbstractValidator<UpdateMemberCommand>
    {
        public UpdateMemberValidator() {

            RuleFor(x=>x.MemberId)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.FirstName)
                       .NotEmpty().WithMessage("First name is required.")
                       .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Le numéro de téléphone est requis.")
                .Matches(@"^(?:\+212|0)(5|6|7)\d{8}$").WithMessage("Numéro de téléphone invalide.")
                .MaximumLength(20)
                .WithMessage("Le numéro de téléphone ne doit pas dépasser 20 caractères.");

            // Validate Address only if it's not null
            When(x => x.AddressDto is not null, () =>
            {
                RuleFor(x => x.AddressDto!).SetValidator(new AddressDtoValidator());
            });
        }
    }
}
