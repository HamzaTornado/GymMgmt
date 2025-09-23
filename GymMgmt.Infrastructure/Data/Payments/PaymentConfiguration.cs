using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Payments;
using GymMgmt.Domain.Entities.Subsciptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Data.Payments
{
    internal class PaymentConfiguration : AuditableEntityConfiguration<Payment,PaymentId>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            base.Configure(builder);

            builder.ToTable("Payments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, value => PaymentId.FromValue(value))
                .IsRequired();

            // --- Configure MemberId Property ---
            builder.Property(p => p.MemberId)
                .HasConversion(id => id.Value, value => MemberId.FromValue(value))
                .HasColumnName("MemberId") // Explicitly map to column
                .IsRequired();

            // --- Configure Member Relationship (PRIMARY RELATIONSHIP) ---
            // This is the CRITICAL PART: Link the FK property and navigation property explicitly.
            builder.HasOne(p => p.Member) 
                .WithMany(m=>m.Payments) 
                .HasForeignKey("MemberId") // IMPORTANT: Use the STRING NAME of the FK property
                .OnDelete(DeleteBehavior.Cascade);

            // Subscription relationship
            builder.Property(x => x.SubscriptionId)
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value.HasValue ? SubscriptionId.FromValue(value.Value) : null)
                .HasColumnName("SubscriptionId")
                .IsRequired(false);

            builder.HasOne<Subscription>()
                .WithMany()
                .HasForeignKey(x => x.SubscriptionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);


            // Payments Properties
            builder.Property(x=> x.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(x=>x.Type)
                .HasColumnName("Type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x=>x.Status)
                .HasColumnName("Status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PaymentDate)
                .HasColumnName("PaymentDate")
                .IsRequired();

            builder.Property(x=>x.PeriodStart)
                .HasColumnName("PeriodStart")
                .IsRequired();

            builder.Property(x => x.PeriodEnd)
                .HasColumnName("PeriodEnd")
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasColumnName("Notes")
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(x => x.Reference)
                .HasColumnName("Reference")
                .IsRequired(false);

            // Indexes for performance
            builder.HasIndex(x => x.MemberId)
                .HasDatabaseName("IX_Payments_MemberId");

            builder.HasIndex(x => x.SubscriptionId)
                .HasDatabaseName("IX_Payments_SubscriptionId");

            builder.HasIndex(x => x.PaymentDate)
                .HasDatabaseName("IX_Payments_PaymentDate");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Payments_Status");



        }
    }
}
