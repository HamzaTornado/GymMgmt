namespace GymMgmt.Api.Middlewares.Responses
{
    /// <summary>
    /// Standardized API response wrapper for consistent HTTP responses.
    /// </summary>
    /// <typeparam name="T">The type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Errors { get; set; }
        public object? Metadata { get; set; }
        public string? CorrelationId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Private constructor to enforce factory pattern
        private ApiResponse() { }

        #region Success Factory Methods

        /// <summary>
        /// Creates a successful response with data.
        /// </summary>
        public static ApiResponse<T> Success(T data, string? message = null, object? metadata = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message ?? "Operation completed successfully",
                Data = data,
                Metadata = metadata
            };
        }

        /// <summary>
        /// Creates a successful response without data.
        /// </summary>
        public static ApiResponse<T> Success(string message)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message
            };
        }

        #endregion

        #region Error Factory Methods

        /// <summary>
        /// Creates a failure response.
        /// </summary>
        public static ApiResponse<T> Fail(string message, object? errors = null, string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates a validation error response.
        /// </summary>
        public static ApiResponse<T> ValidationError(
            string message,
            Dictionary<string, string[]> validationErrors,
            string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = validationErrors,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates a not found response.
        /// </summary>
        public static ApiResponse<T> NotFound(string message = "Resource not found", string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates an unauthorized response.
        /// </summary>
        public static ApiResponse<T> Unauthorized(string message = "Unauthorized access", string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates a forbidden response.
        /// </summary>
        public static ApiResponse<T> Forbidden(string message = "Access forbidden", string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates a conflict response.
        /// </summary>
        public static ApiResponse<T> Conflict(string message = "Resource conflict", string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                CorrelationId = correlationId
            };
        }

        /// <summary>
        /// Creates a server error response.
        /// </summary>
        public static ApiResponse<T> ServerError(string message = "Internal server error", object? details = null, string? correlationId = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = details,
                CorrelationId = correlationId
            };
        }

        #endregion
    }
}
