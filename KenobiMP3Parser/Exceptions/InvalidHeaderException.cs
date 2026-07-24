namespace KenobiMp3Parser.Exceptions
{
    // The exceptions need some work... currently in a very raw state.
    public class InvalidHeaderException(byte[] failedBytes, long position) : Exception
    {
        public readonly byte[] FailedBytes = failedBytes;
        public readonly long FailedPosition = position;
    }
}
