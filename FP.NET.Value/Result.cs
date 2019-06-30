using System;

namespace FP.NET.Value
{
    public class Result
    {
        public string Error { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException();
            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException();

            Error = error;
            IsSuccess = isSuccess;
        }

        public static Result Ok() => new Result(true, string.Empty);
        public static Result Fail(string message) => new Result(false, message);
        public static Result<T> Ok<T>(T value) => new Result<T>(value, true, string.Empty);
        public static Result<T> Fail<T>(string message) => new Result<T>(default(T), false, message);
    }

    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
}