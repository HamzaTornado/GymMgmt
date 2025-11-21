using FluentValidation;

namespace GymMgmt.Application.Features.Memberships.CreateMembership
{
    internal class CreateMemberShipPlanValidator : AbstractValidator<CreateMemberShipPlanCommand>
    {
        public CreateMemberShipPlanValidator() { 
        
            RuleFor(p=>p.Name)
                .NotEmpty().WithMessage("Name is Required");

            RuleFor(p => p.DurationInMonths)
                .NotEmpty().WithMessage("Duration is required")
                .GreaterThan(0).WithMessage("Duration must be Valid");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Price is required")
                .GreaterThan(0).WithMessage("Price must be Valid");
                
        }
    }
}
