using GymMgmt.Domain.Entities.ClubSettingsConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Data.ClubSettingsConfig
{
    public class ClubSettingsRepository(GymDbContext dbcontext) :BaseRepository<ClubSettings,ClubSettingsId>(dbcontext), IClubSettingsRepository
    {
    }
}
