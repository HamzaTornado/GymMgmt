

namespace GymMgmt.Application.Common.Exceptions
{
    public class UnauthorizedException : ApplicationLayerException
    {
        public UnauthorizedException(string message = "Unauthorized access.")
            : base("UNAUTHORIZED", message)
        {
        }

        public UnauthorizedException(string message, Exception innerException)
            : base("UNAUTHORIZED", message, innerException)
        {
        }
    }
}
