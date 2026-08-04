using KenobiMp3Parser.Classes;

namespace KenobiMp3Parser.Exceptions
{
    // The exceptions need some work... currently in a very raw state.
    public class InvalidFileException(FrameStatus status, string? message = null) : Exception
    {
        public override string Message => message ?? base.Message;
        public readonly FrameStatus Status = status;
    }
}
