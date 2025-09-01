using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Identity
{
    public class JwtSettings
    {
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public long TokenValidityInMinutes { get; set; }
        public long RefreshTokenValidityInMinutes { get; set; }
        public string Secret { get; set; } = string.Empty;
    }
}
