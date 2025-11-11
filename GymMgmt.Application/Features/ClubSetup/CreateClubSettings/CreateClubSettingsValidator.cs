using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.ClubSetup.AddClubSettings
{
    internal class CreateClubSettingsValidator : AbstractValidator<CreateClubSettingsCommand>
    {
        public CreateClubSettingsValidator() {

            RuleFor(s=> s.InsurranceFeeAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Insurrance Fee Amount must be positive");

            RuleFor(s => s.InsuranceValidityInDays)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Insurance validity must be positive");

            RuleFor(s => s.SubscriptionGracePeriodInDays)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Subscription Grace Period must be positive");
            
        }
    }
}
