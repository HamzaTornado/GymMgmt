using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.Queries
{
    public class MemberListDto
    {
        public Guid MemberId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public DateTime EndDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public bool HasActiveSubscription { get; set; } // NEW
        public bool PaidInsurance { get; set; } //NEW
        public int DaysOverdue { get; set; }
    }
}
