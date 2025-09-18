namespace GymMgmt.Application.Common.Exceptions
{
    public class BadRequestException : ApplicationLayerException
    {
        /// <summary>
        /// Thrown when HTTP request is malformed or contains invalid data.
        /// Typically results in 400 Bad Request response.
        /// </summary>
        public BadRequestException()
            : base(
                errorCode: "BAD_REQUEST",
                message: "The request is invalid or malformed.")
        {
        }

        public BadRequestException(string message)
            : base(
                errorCode: "BAD_REQUEST",
                message: message)
        {
        }

        public BadRequestException(string message, Exception? innerException)
            : base(
                errorCode: "BAD_REQUEST",
                message: message,
                innerException: innerException)
        {
        }
    }
}
