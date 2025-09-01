using GymMgmt.Domain.Entities.ClubSettingsConfig;
using GymMgmt.Domain.Entities.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymMgmt.Infrastructure.Data.ClubSettingsConfig
{
    internal class ClubSettingsConfiguration : AuditableEntityConfiguration<ClubSettings, ClubSettingsId>
    {
        public override void Configure(EntityTypeBuilder<ClubSettings> builder)
        {
            base.Configure(builder);
            // Configure the Member entity
            builder.ToTable("ClubSettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => ClubSettingsId.FromValue(value))
            .IsRequired();

            // Insurance Fee(ValueObject) - owned entity
            builder.OwnsOne(x => x.CurrentInsuranceFee, insuranceFee =>
            {
                insuranceFee.Property(f => f.Amount)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("InsuranceFeeAmount");

                insuranceFee.Property(f => f.Currency)
                    .HasMaxLength(3)
                    .HasColumnName("InsuranceFeeCurrency");

                insuranceFee.Property(f => f.IsActive)
                    .HasColumnName("InsuranceFeeIsActive");
            });

            // Boolean flags
            builder.Property(x => x.AreNewMembersAllowed)
                .HasDefaultValue(true)
                .HasColumnName("AreNewMembersAllowed");

            builder.Property(x => x.IsInsuranceFeeRequired)
                .HasDefaultValue(false)
                .HasColumnName("IsInsuranceFeeRequired");

            // Integer settings
            builder.Property(x => x.SubscriptionGracePeriodInDays)
                .HasDefaultValue(5)
                .HasColumnName("SubscriptionGracePeriodInDays");

            builder.Property(x => x.InsuranceValidityInDays)
                .HasDefaultValue(365)
                .HasColumnName("InsuranceValidityInDays");

            // Add database comments (optional)
            builder.ToTable(t => t.HasComment("Global club configuration settings"));

            
        }
    }
}
