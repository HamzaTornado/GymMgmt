using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.UpdateMembershipPlan
{
    internal class UpdateMembershipPlanValidator : AbstractValidator<UpdateMemberShipPlanCommand>
    {
        public UpdateMembershipPlanValidator() {


            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(p => p.Name)
               .NotEmpty().WithMessage("Name is Required");

            RuleFor(p => p.DurationInDays)
                .NotEmpty().WithMessage("Duration is required")
                .GreaterThan(0).WithMessage("Duration must be Valid");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Price is required")
                .GreaterThan(0).WithMessage("Price must be Valid");

        }
    }
}
