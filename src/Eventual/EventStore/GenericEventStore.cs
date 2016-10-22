namespace Eventual.EventStore
{
    internal enum ShouldRetry
    {
        Yes,
        No
    }

    internal enum ExitStatus
    {
        Completed,
        TooManyRetries
    }

    public class ConcurrencyEventInfo
    {
        public int Sequence { get; set; }
        public string TypeName { get; set; }
    }

    public class StreamAppendAttempt
    {
        public Result Result { get; }

        public int Sequence { get; }

        public static StreamAppendAttempt Success()
        {
            return new StreamAppendAttempt(Result.Success, 0);
        }

        public static StreamAppendAttempt Failure(int sequence)
        {
            return new StreamAppendAttempt(Result.Failure, sequence);
        }

        protected StreamAppendAttempt(Result result, int sequence)
        {
            this.Result = result;
            this.Sequence = sequence;
        }
    }

    public enum Result
    {
        Success,
        Failure
    }
}