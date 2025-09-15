namespace GymMgmt.Api.Middlewares.Exceptions
{
    public class EmptyConnectionStringException : ApiLayerException
    {
        public EmptyConnectionStringException()
        : base(
            errorCode: "EMPTY_CONNECTION_STRING",
            message: "The connection string is missing or empty.")
        {
        }
    }
}
