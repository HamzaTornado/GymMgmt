using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Common.ValueObjects
{
    public sealed record Address(
       string Street,
       string City,
       string Region,
       string PostalCode
       );
}
