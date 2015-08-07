namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public class ClipboardEntry
    {
        public UInt16 Format { get; private set; }
        public String Name { get; private set; }
        public UInt64 DataSize { get; private set; }

        public ClipboardEntry(UInt16 format, String name, UInt64 dataSize)
        {
            Format = format;
            Name = name;
            DataSize = dataSize;
        }
    }
}
