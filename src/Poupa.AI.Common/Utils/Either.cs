namespace Poupa.AI.Common.Utils
{
    public class Either<TError, TSuccess>
    {
        public TError? Error { get; }
        public TSuccess? Success { get; }
        public bool IsError { get; }
        public bool IsSuccess => !IsError;

        private Either(TError error)
        {
            Error = error;
            IsError = true;
        }

        private Either(TSuccess success)
        {
            Success = success;
            IsError = false;
        }

        public static Either<TError, TSuccess> FromError(TError error)
            => new(error);

        public static Either<TError, TSuccess> FromSuccess(TSuccess success)
            => new(success);
    }
}
