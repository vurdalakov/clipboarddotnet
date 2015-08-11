namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public class EntryViewModel : ViewModelBase
    {
        public UInt16 Format { get { return _clipboardEntry.Id; } }
        public UInt64 DataSize { get { return _clipboardEntry.DataSize; } }
        public String Name { get { return _clipboardEntry.Name; } }

        private ClipboardEntry _clipboardEntry;

        public EntryViewModel(ClipboardEntry clipboardEntry)
        {
            _clipboardEntry = clipboardEntry;
        }
    }
}
