namespace AuthApi.Models.Utilities
{
    public class Result<T>
    {
        public bool IsSuccessful { get; }
        public string Error { get; }
        public T Value { get; }

        private Result(T value)
        {
            IsSuccessful = true;
            Value = value;
        }

        private Result(string error)
        {
            IsSuccessful = false;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(string error) => new Result<T>(error);
    }

}
