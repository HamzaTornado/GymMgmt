using FluentValidation.Results;


namespace GymMgmt.Application.Common.Exceptions
{
    public class ValidationException : ApplicationLayerException
    {
        public IDictionary<string, string[]> Errors { get; }
        public ValidationException()
            : base(
                  errorCode: "VALIDATION_FAILED",
                  message: "One or more validation failures have occurred."
                  )

        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.Distinct().ToArray());
        }
    }
}
