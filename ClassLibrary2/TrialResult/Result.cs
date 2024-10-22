using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models.TrialResult
{
    // Sealed record for error handling
    public sealed record Error(string Code, string? Description = null)
    {
        public static readonly Error None = new(string.Empty);
        public static readonly Error ExceptionError = new("Exception occured", "The inner details are in the exception");
    }

    // Non-generic Result class
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error state", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }


        public bool IsSuccess { get; set; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        // Factory methods for success/failure
        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
    }

    // Generic Result class
    public class Result<TData> : Result where TData : class
    {
        private readonly Exception? _exception;
        protected Result(TData? data, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            Data = data;
        }

        protected Result(Exception? exception, bool isSuccess, Error error)
            : base(isSuccess, error)
        {

            _exception = exception;
        }

        public TData? Data { get; }

        // Factory method for success with data
        public static Result<TData> Success(TData data) => new(data, true, Error.None);

        // Factory method for failure with error
        public static Result<TData> Failure(Exception exception) => new(exception, false, Error.ExceptionError);
    }
}
