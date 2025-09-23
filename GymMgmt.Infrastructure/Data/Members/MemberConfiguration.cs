using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GymMgmt.Infrastructure.Data.Members
{
    internal class MemberConfiguration : AuditableEntityConfiguration<Member, MemberId>
    {
        public override void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder);
            // Configure the Member entity
            builder.ToTable("Members");

            builder.HasKey(b => b.Id);

            builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => MemberId.FromValue(value))
            .IsRequired();

            builder.HasMany(m => m.Payments)
            .WithOne(p => p.Member)  // ← CRITICAL FIX  
            .HasForeignKey(p => p.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

            // Map private properties
            builder.Property(m => m.FirstName)
                .HasColumnName("FirstName")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.LastName)
                .HasColumnName("LastName")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Email)
                .HasColumnName("Email")
                .HasMaxLength(250);

            builder.Property(m => m.PhoneNumber)
                 .HasConversion(
                     v => v.Value,               // to DB
                     v => PhoneNumber.Create(v) // from DB
                 )
                 .HasMaxLength(20)
                 .HasColumnName("PhoneNumber");

            builder.OwnsOne(m => m.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(100);

                addressBuilder.Property(a => a.City)
                    .HasColumnName("City")
                    .HasMaxLength(100);

                addressBuilder.Property(a => a.Region)
                    .HasColumnName("Region")
                    .HasMaxLength(100);

                addressBuilder.Property(a => a.PostalCode)
                    .HasColumnName("PostalCode")
                    .HasMaxLength(20);
            });

        }
    }
}

