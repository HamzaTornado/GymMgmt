using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using MediatR;

namespace GymMgmt.Application.Features.ClubSetup.GetClubSettings
{
    public class GetClubSettingsQueryHandler : IRequestHandler<GetClubSettingsQuery,ReadClubSettingsDto>
    {
        private readonly IClubSettingsRepository _clubSettingsRepository;
        private readonly IMapper _mapper;


        public GetClubSettingsQueryHandler(IClubSettingsRepository clubSettingsRepository ,IMapper mapper) {
            
            _clubSettingsRepository = clubSettingsRepository;
            _mapper = mapper;
        }

        public async Task<ReadClubSettingsDto> Handle(GetClubSettingsQuery request,CancellationToken cancellationToken)
        {
            var clubSettings = await _clubSettingsRepository.GetSingleOrDefaultAsync(cancellationToken);

            if (clubSettings == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<ReadClubSettingsDto>(clubSettings);
        }
    }
}
