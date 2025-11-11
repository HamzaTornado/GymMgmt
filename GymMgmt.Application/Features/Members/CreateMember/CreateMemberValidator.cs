using AuthSystem.Application.Features.Memebers;
using FluentValidation;


namespace GymMgmt.Application.Features.Members.CreateMember
{
    internal class CreateMemberValidator :AbstractValidator<CreateMemberCommand>
    {
        public CreateMemberValidator() {

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Le numéro de téléphone est requis.")
                .Matches(@"^(?:\+212|0)(5|6|7)\d{8}$")
                    .WithMessage("Numéro de téléphone invalide.")
                .MaximumLength(20)
                    .WithMessage("Le numéro de téléphone ne doit pas dépasser 20 caractères.");

            // Validate Address only if it's not null
            When(x => x.Address is not null, () =>
            {
                RuleFor(x => x.Address!).SetValidator(new AddressDtoValidator());
            });

        }
    }
}
