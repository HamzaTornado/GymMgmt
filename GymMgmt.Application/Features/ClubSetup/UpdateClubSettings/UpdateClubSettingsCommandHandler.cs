using AutoMapper;
using GymMgmt.Application.Common.Exceptions;
using GymMgmt.Application.Features.ClubSetup.GetClubSettings;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.ClubSetup.UpdateClubSettings
{
    internal class UpdateClubSettingsCommandHandler : IRequestHandler<UpdateClubSettingsCommand, ReadClubSettingsDto>
    {

        private readonly IClubSettingsRepository _clubSettingsRepository;
        private readonly IMapper _mapper;

        public UpdateClubSettingsCommandHandler(IClubSettingsRepository clubSettingsRepository, IMapper mapper)
        {
            _clubSettingsRepository = clubSettingsRepository;
            _mapper = mapper;
        }
        public async Task<ReadClubSettingsDto> Handle(UpdateClubSettingsCommand request, CancellationToken cancellationToken)
        {
            var existingSettings = await _clubSettingsRepository.FindByIdAsync(ClubSettingsId.FromValue(request.ClubSettingsId), cancellationToken);

            if (existingSettings == null) {
                throw new NotFoundException();
            }

            existingSettings.UpdateSubscriptionGracePeriod(request.SubscriptionGracePeriodInDays);
            existingSettings.UpdateInsuranceValidity(request.InsuranceValidityInDays);
            existingSettings.SetInsuranceFeeRequirement(request.IsInsuranceFeeRequired);
            existingSettings.SetNewMembersAllowed(request.AreNewMembersAllowed);


            if (existingSettings.CurrentInsuranceFee.Amount != request.InsurranceFeeAmount)
            {
                var newInsuranceFee = new InsuranceFee(request.InsurranceFeeAmount);
                existingSettings.UpdateInsuranceFee(newInsuranceFee);
            }
            

            return _mapper.Map<ReadClubSettingsDto>(existingSettings);
        }
    }
}
