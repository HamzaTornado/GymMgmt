using GymMgmt.Domain.Entities.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.ClubSettingsConfig
{
    public record ClubSettingsId(Guid Value)
    {
        public static ClubSettingsId FromValue(Guid value) => new(value);
        public static ClubSettingsId New() => FromValue(Guid.NewGuid());
    }
}
