using GymMgmt.Domain.Entities.Subsciptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Data.Subscriptions
{
    internal class SubscriptionConfiguration : AuditableEntityConfiguration<Subscription,SubscriptionId>
    {
        public override void Configure(EntityTypeBuilder<Subscription> builder)
        {
            base.Configure(builder);

            builder.ToTable("Subscriptions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, value => SubscriptionId.FromValue(value))
                .IsRequired();

            builder.Property(x => x.StartDate)
                .HasColumnName("StartDate")
                .IsRequired();

            builder.Property(x => x.EndDate)
                .HasColumnName("EndDate")
                .IsRequired();

            builder.Property(x=>x.Status)
                .HasColumnName ("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

        }
    }
}
