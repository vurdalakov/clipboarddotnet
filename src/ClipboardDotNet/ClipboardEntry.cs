namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public class ClipboardEntry : ClipboardFormat
    {
        public UInt64 DataSize { get; private set; }

        public ClipboardEntry(UInt16 id, String name, UInt64 dataSize) : base(id, name)
        {
            DataSize = dataSize;
        }
    }
}
