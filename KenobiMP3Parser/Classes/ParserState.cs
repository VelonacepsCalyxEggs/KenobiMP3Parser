using System;
using System.Collections.Generic;
using System.Text;

namespace KenobiMp3Parser.Classes
{
    internal ref struct ParserState
    {
        public int FailedFrames;
        public int FailedFramesConsecutive;
        public bool PreviousFailed;
        public int FramesRead;
        public Mp3Frame PrevFrame;
        public bool HasPrevFrame;

        public void RegisterValidFrame(Mp3Frame frame)
        {
            FramesRead++;
            PreviousFailed = false;
            FailedFramesConsecutive = 0;
            PrevFrame = frame;
            HasPrevFrame = true;
        }

        public void RegisterFrameError(bool isConsecutiveOnly)
        {
            if (!isConsecutiveOnly)
            {
                FailedFrames++;
            }

            if (PreviousFailed)
            {
                FailedFramesConsecutive++;
            }
            PreviousFailed = true;
        }
    }
}
