using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Identity
{
    public class JwtSigningConfigurations
    {
        public JwtSigningConfigurations(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            SecurityKey = new SymmetricSecurityKey(keyBytes);
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }
        public SecurityKey SecurityKey { get; }
        public SigningCredentials SigningCredentials { get; }
    }
}
