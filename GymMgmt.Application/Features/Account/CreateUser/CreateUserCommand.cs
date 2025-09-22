using AuthSystem.Application.Features.Memebers;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using GymMgmt.Application.Features.Account.GetUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Account.CreateUser
{
    public sealed record CreateUserCommand(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email,
        string Password,
        string ConfirmPassword,
        List<string> Roles
        ) : ICommand<RegistreResponse>
    {
    }
}
