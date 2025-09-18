using FluentValidation.Results;
using GymMgmt.Application.Common.Models;


namespace GymMgmt.Application.Common.Exceptions
{
    public class IdentityValidationException : ApplicationLayerException
    {
        public IDictionary<string, string[]> Errors { get; }
        public IdentityValidationException()
        : base(
            errorCode: "IDENTITY_VALIDATION_FAILED",
            message: "One or more identity validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public IdentityValidationException(IEnumerable<ValidationFailure> failures)
        : this()
        {
            if (failures == null) return;

            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray());
        }

        public IdentityValidationException(IEnumerable<AppIdentityError> errors)
        : this()
        {
            if (errors == null) return;

            Errors = errors
                .GroupBy(e => e.Code, e => e.Description)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray());
        }
    }
}
