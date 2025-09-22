using GymMgmt.Domain.Entities.ClubSettingsConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Data.ClubSettingsConfig
{
    public class ClubSettingsRepository(GymDbContext dbcontext) : BaseRepository<ClubSettings, ClubSettingsId>(dbcontext), IClubSettingsRepository
    {
        public async Task<ClubSettings?> GetSingleOrDefaultAsync(CancellationToken ct = default)
        {
            return await _dbContext.Set<ClubSettings>().FirstOrDefaultAsync(ct);
        }
    }
}
