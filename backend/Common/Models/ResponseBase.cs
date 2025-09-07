using backend.Common.Entities;

namespace backend.Common.Models
{
    public class ResponseBase
    {
        public ResponseStatusCode Code { get; set; }
        public bool Success => (int)Code >= 200 && (int)Code < 300;
        public string? Message { get; set; }
        public object? Error { get; set; }

        public ResponseBase() { }

        public ResponseBase(ResponseStatusCode code, string? message = null, object? error = null)
        {
            Code = code;
            Message = message;
            Error = error;
        }

        public static ResponseBase Ok(string? message = null)
            => new(ResponseStatusCode.Success, message);

        public static ResponseBase Fail(ResponseStatusCode code, string? message = null, object? error = null)
            => new(code, message, error);
    }

    public class ResponseBase<T> : ResponseBase
    {
        public T? Data { get; set; }

        public ResponseBase() : base() { }

        public ResponseBase(ResponseStatusCode code, string? message = null, T? data = default, object? error = null)
            : base(code, message, error)
        {
            Data = data;
        }

        public static ResponseBase<T> Ok(T? data = default, string? message = null)
            => new(ResponseStatusCode.Success, message, data);

        public new static ResponseBase<T> Fail(ResponseStatusCode code, string? message = null, object? error = null)
            => new(code, message, default, error);
    }
}