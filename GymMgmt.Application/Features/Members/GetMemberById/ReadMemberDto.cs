using AuthSystem.Application.Features.Memebers;

namespace GymMgmt.Application.Features.Members.GetMemberById
{
    public class ReadMemberDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }

        public AddressDto? Address { get; set; }
        public bool HasPaidInsurance { get; set; }
    }
}
