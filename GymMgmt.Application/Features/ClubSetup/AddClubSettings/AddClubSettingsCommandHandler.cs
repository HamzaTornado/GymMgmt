using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Features.ClubSetup.GetClubSettings;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using MediatR;

namespace GymMgmt.Application.Features.ClubSetup.AddClubSettings
{
    public class AddClubSettingsCommandHandler : IRequestHandler<AddClubSettingsCommand, ReadClubSettingsDto>
    {
        private readonly IClubSettingsRepository _clubSettingsRepository;
        private readonly IMapper _mapper;

        public AddClubSettingsCommandHandler(IClubSettingsRepository clubSettingsRepository, IMapper mapper)
        {
            _clubSettingsRepository = clubSettingsRepository;
            _mapper = mapper;
        }

        public async Task<ReadClubSettingsDto> Handle(AddClubSettingsCommand request, CancellationToken cancellationToken)
        {
            var existClubSetting = _clubSettingsRepository.GetSingleOrDefaultAsync(cancellationToken);
            
            if (existClubSetting != null) {
                throw new ConflictException();
            }
            // creating a InsuranceFee ValuObject
            var insuranceFee = new InsuranceFee(request.InssurranceFeeAmount);

            var clubSettings = ClubSettings.Create(insuranceFee);

            await _clubSettingsRepository.AddAsync(clubSettings,cancellationToken);

            return _mapper.Map<ReadClubSettingsDto>(clubSettings);

        }
    }
}
