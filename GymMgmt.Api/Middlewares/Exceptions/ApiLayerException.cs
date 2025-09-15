namespace GymMgmt.Api.Middlewares.Exceptions
{
    public class ApiLayerException : Exception
    {
        public string ErrorCode { get; }
        public ApiLayerException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }

        public ApiLayerException(string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }
    }
}
