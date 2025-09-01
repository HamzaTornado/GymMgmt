
namespace GymMgmt.Application.Common.Results
{
    public class AppIdentityResult
    {
        public bool Succeeded { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        private AppIdentityResult(bool succeeded, IEnumerable<string>? errors = null)
        {
            Succeeded = succeeded;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public static AppIdentityResult Success() => new(true);
        public static AppIdentityResult Failure(params string[] errors) => new(false, errors);
    }
}
