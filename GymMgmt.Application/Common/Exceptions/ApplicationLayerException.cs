namespace GymMgmt.Application.Common.Exceptions
{
    public class ApplicationLayerException : Exception
    {
        public string ErrorCode { get; }
        public ApplicationLayerException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }

        public ApplicationLayerException(string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }
    }
}
