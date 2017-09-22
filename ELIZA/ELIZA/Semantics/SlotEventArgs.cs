using System;

namespace ELIZA.Semantics
{
    public class SlotEventArgs: EventArgs
    {
        public Frame CallingFrame
        {
            get;
            private set;
        }

        public SlotEventArgs(Frame frame)
        {
            CallingFrame = frame;
        }
    }
}
