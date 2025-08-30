using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMgmt.Domain.Common;

namespace GymMgmt.Infrastructure.Data
{
    internal abstract class AuditableEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
           where TEntity : AuditableEntity<TId>
           where TId : notnull 
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.CreatedBy).HasMaxLength(100);
            builder.Property(e => e.LastModified);
            builder.Property(e => e.LastModifiedBy).HasMaxLength(100);
        }
    }
}
