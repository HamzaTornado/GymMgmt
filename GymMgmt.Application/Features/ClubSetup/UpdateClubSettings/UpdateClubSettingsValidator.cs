using FluentValidation;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.ClubSetup.UpdateClubSettings
{
    internal class UpdateClubSettingsValidator : AbstractValidator<UpdateClubSettingsCommand>
    {
        private readonly IClubSettingsRepository _repository;
        public UpdateClubSettingsValidator(IClubSettingsRepository repository) {

            _repository = repository;
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(s => s.ClubSettingsId)
                .NotNull().WithMessage("Club Settings ID is required")
                .MustAsync(ExistAsync).WithMessage("Club Settings with this ID does not exist");

            RuleFor(s => s.InsurranceFeeAmount)
                .GreaterThan(0)
                .WithMessage("Insurrance Fee Amount must be grater than 0");

            RuleFor(s => s.InsuranceValidityInDays)
                .GreaterThan(0)
                .WithMessage("Insurance validity must be grater than 0");

            RuleFor(s => s.SubscriptionGracePeriodInDays)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Subscription Grace Period must be positive");

        }
        private async Task<bool> ExistAsync(Guid clubSettingsId, CancellationToken cancellationToken)
        {
            var exists = await _repository.FindByIdAsync(ClubSettingsId.FromValue(clubSettingsId),cancellationToken);
            return exists !=null ;
        }
    }
}
