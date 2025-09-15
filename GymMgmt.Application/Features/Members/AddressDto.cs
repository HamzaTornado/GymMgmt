using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Application.Features.Memebers
{
    public record AddressDto(
    string Street,
    string City,
    string Region,
    string PostalCode);
}
