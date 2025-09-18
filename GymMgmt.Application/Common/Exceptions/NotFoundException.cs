using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Exceptions
{
    public class NotFoundException : ApplicationLayerException
    {
        public string? EntityName { get; }
        public object? Key { get; }

        public NotFoundException()
            : base(
                errorCode: "NOT_FOUND",
                message: "The requested resource was not found.")
        {
        }

        public NotFoundException(string message)
            : base(
                errorCode: "NOT_FOUND",
                message: message)
        {
        }

        public NotFoundException(string entityName, object key)
            : base(
                errorCode: "ENTITY_NOT_FOUND",
                message: $"Entity \"{entityName}\" with the identifier ({key}) was not found.")
        {
            EntityName = entityName;
            Key = key;
        }
    }
}
