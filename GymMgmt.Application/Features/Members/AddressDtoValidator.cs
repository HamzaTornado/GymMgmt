using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Application.Features.Memebers
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(100).WithMessage("Street must not exceed 100 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required.")
                .MaximumLength(50).WithMessage("Region must not exceed 50 characters.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal code is required.")
                .MaximumLength(20).WithMessage("Postal code must not exceed 20 characters.");
        }
    }
}
