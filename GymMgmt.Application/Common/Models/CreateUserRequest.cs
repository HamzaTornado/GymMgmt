using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Models
{
    public record CreateUserRequest(string FirstName, string LastName, string Password,string ConfirmPassword,string PhoneNumber, string Email, List<string> Roles);
}
