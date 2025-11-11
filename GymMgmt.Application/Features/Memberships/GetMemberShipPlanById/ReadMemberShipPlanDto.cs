using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.GetMemberShipPlanById
{
    public class ReadMemberShipPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DurationInDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
