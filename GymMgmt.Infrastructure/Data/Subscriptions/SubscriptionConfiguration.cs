using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Subsciptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


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

            builder.Property(p => p.MemberId)
                .HasConversion(id => id.Value, value => MemberId.FromValue(value))
                .HasColumnName("MemberId") // Explicitly map to column
                .IsRequired();

            
            builder.HasOne(s => s.Member)
                .WithMany(m => m.Subscriptions)
                .HasForeignKey(s=>s.MemberId) 
                .OnDelete(DeleteBehavior.Cascade);

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

            // Indexes for performance
            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Subscriptions_Status");

            builder.HasIndex(x => x.EndDate)
                .HasDatabaseName("IX_Subscriptions_EndDate");
        }
    }
}
