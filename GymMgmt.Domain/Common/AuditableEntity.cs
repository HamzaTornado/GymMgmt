using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Common
{
    /// <summary>
    /// Base class for entities that support audit tracking.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's ID.</typeparam>
    public abstract class AuditableEntity<TId> : Entity<TId>, IAuditableEntity where TId : notnull
    {
        // These setters are private — only modifiable via internal methods or interceptor
        public DateTimeOffset Created { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTimeOffset? LastModified { get; private set; }
        public string? LastModifiedBy { get; private set; }


        // Called by the interceptor — not meant for public use
        public void SetCreationInfo(DateTimeOffset created, string? createdBy)
        {
            Created = created;
            CreatedBy = createdBy;
        }

        public void SetModificationInfo(DateTimeOffset modified, string? modifiedBy)
        {
            LastModified = modified;
            LastModifiedBy = modifiedBy;
        }
    }
}
