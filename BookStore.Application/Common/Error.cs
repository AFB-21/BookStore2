using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Common
{
    public sealed class Error
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        // Common errors
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

        public static Error NotFound(string entityName, object id) =>
            new("Error.NotFound", $"{entityName} with id '{id}' was not found.");

        public static Error Validation(string message) =>
            new("Error.Validation", message);

        public static Error Unauthorized(string message = "Unauthorized access.") =>
            new("Error.Unauthorized", message);

        public static Error Forbidden(string message = "Access forbidden.") =>
            new("Error.Forbidden", message);

        public static Error Conflict(string message) =>
            new("Error.Conflict", message);
    }
}
