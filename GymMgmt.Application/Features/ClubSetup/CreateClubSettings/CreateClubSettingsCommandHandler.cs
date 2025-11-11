using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Features.ClubSetup.GetClubSettings;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using MediatR;

namespace GymMgmt.Application.Features.ClubSetup.AddClubSettings
{
    public class CreateClubSettingsCommandHandler : IRequestHandler<CreateClubSettingsCommand, ReadClubSettingsDto>
    {
        private readonly IClubSettingsRepository _clubSettingsRepository;
        private readonly IMapper _mapper;

        public CreateClubSettingsCommandHandler(IClubSettingsRepository clubSettingsRepository, IMapper mapper)
        {
            _clubSettingsRepository = clubSettingsRepository;
            _mapper = mapper;
        }

        public async Task<ReadClubSettingsDto> Handle(CreateClubSettingsCommand request, CancellationToken cancellationToken)
        {
            var existClubSetting = await _clubSettingsRepository.GetSingleOrDefaultAsync(cancellationToken);
            
            if (existClubSetting != null) {
                throw new ConflictException();
            }
            // creating a InsuranceFee ValuObject 

            var insuranceFee = new InsuranceFee(request.InsurranceFeeAmount);
            // on creation if the insuranceFee if empty it will add the default value of 100DH in domain layer
            var clubSettings = ClubSettings.Create(insuranceFee);

            if (request.ParDefaut)
            {
                clubSettings.UpdateInsuranceValidity(request.InsuranceValidityInDays);
                clubSettings.UpdateSubscriptionGracePeriod(request.SubscriptionGracePeriodInDays);
                clubSettings.SetInsuranceFeeRequirement(request.IsInsuranceFeeRequired);
                clubSettings.SetNewMembersAllowed(request.AreNewMembersAllowed);
            }
            
            await _clubSettingsRepository.AddAsync(clubSettings,cancellationToken);

            return _mapper.Map<ReadClubSettingsDto>(clubSettings);

        }
    }
}
