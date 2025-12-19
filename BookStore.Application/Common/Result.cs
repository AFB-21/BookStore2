namespace BookStore.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("Successful result cannot have an error.");

            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("Failed result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        // Factory methods
        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result NotFound(string entityName, object id) =>
            Failure(Error.NotFound(entityName, id));

        public static Result Validation(string message) =>
            Failure(Error.Validation(message));

        public static Result Unauthorized(string message = "Unauthorized access.") =>
            Failure(Error.Unauthorized(message));

        public static Result Forbidden(string message = "Access forbidden.") =>
            Failure(Error.Forbidden(message));

        public static Result Conflict(string message) =>
            Failure(Error.Conflict(message));
    }
}
