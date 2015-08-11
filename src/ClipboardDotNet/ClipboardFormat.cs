namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public class ClipboardFormat
    {
        public UInt16 Id { get; private set; }
        public String Name { get; private set; }

        public ClipboardFormat(UInt16 id, String name)
        {
            Id = id;
            Name = name;
        }
    }
}
