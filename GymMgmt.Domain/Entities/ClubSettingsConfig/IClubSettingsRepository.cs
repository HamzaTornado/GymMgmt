using GymMgmt.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.ClubSettingsConfig
{
    public interface IClubSettingsRepository : IBaseRepository<ClubSettings,ClubSettingsId>
    {
        Task<ClubSettings?> GetSingleOrDefaultAsync(CancellationToken ct = default);
    }
}
