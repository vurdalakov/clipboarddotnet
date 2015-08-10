namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public class HexLineViewModel : ViewModelBase
    {
        public Int32 Offset { get; private set; }
        public Byte[] Data { get; private set; }

        public HexLineViewModel(Int32 offset, Byte[] data)
        {
            Offset = offset;
            Data = data;
        }
    }
}
