using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Common
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access value of a failed result.");

        protected internal Result(T? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        // Factory methods
        public static Result<T> Success(T value) => new(value, true, Error.None);

        public static new Result<T> Failure(Error error) => new(default, false, error);

        public static new Result<T> NotFound(string entityName, object id) =>
            Failure(Error.NotFound(entityName, id));

        public static new Result<T> Validation(string message) =>
            Failure(Error.Validation(message));

        public static new Result<T> Unauthorized(string message = "Unauthorized access.") =>
            Failure(Error.Unauthorized(message));

        public static new Result<T> Forbidden(string message = "Access forbidden.") =>
            Failure(Error.Forbidden(message));

        public static new Result<T> Conflict(string message) =>
            Failure(Error.Conflict(message));

        // Implicit conversion from T to Result<T>
        public static implicit operator Result<T>(T value) => Success(value);
    }
}
