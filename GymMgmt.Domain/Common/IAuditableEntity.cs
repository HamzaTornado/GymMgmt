using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Common
{
    /// <summary>
    /// Represents an entity that tracks creation and modification metadata.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets the date and time the entity was created.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// Gets the user who created the entity.
        /// </summary>
        string? CreatedBy { get; }

        /// <summary>
        /// Gets the date and time the entity was last modified.
        /// </summary>
        DateTimeOffset? LastModified { get; }

        /// <summary>
        /// Gets the user who last modified the entity.
        /// </summary>
        string? LastModifiedBy { get; }
    }
}
