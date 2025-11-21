using GymMgmt.Domain.Entities.Plans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Data.Plans
{
    internal class MemberShipPlanConfiguration : AuditableEntityConfiguration<MembershipPlan,MembershipPlanId>
    {
        public override void Configure(EntityTypeBuilder<MembershipPlan> builder)
        {
            base.Configure(builder);
            builder.ToTable("MemberShipPlans");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, value => MembershipPlanId.FromValue(value))
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x=>x.DurationInMonths)
                .HasColumnName("DurationInMonths")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
