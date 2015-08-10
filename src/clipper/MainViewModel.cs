namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Controls;

    public class MainViewModel : ViewModelBase
    {
        public ThreadSafeObservableCollection<EntryViewModel> Entries { get; private set; }

        private EntryViewModel _selectedEntry;
        public EntryViewModel SelectedEntry
        {
            get
            {
                return _selectedEntry;
            }
            set
            {
                if (_selectedEntry != value)
                {
                    _selectedEntry = value;
                    this.OnPropertyChanged(() => SelectedEntry);

                    UpdateHexData();
                }
            }
        }

        public ThreadSafeObservableCollection<HexLineViewModel> HexLines { get; private set; }

        public String SequenceNumber { get; private set; }

        private ClipboardListener _clipboardListener = new ClipboardListener();

        public MainViewModel()
        {
            Entries = new ThreadSafeObservableCollection<EntryViewModel>();
            HexLines = new ThreadSafeObservableCollection<HexLineViewModel>();

            _clipboardListener.ClipboardUpdated += (s, e) => Refresh();
        }

        public void OnLoaded()
        {
            _clipboardListener.Start();

            Refresh();
        }

        public void OnClosing()
        {
            _clipboardListener.Stop();
        }

        public void Refresh()
        {
            var selectedFormat = null == SelectedEntry ? 0 : SelectedEntry.Format;

            Entries.Clear();
            SelectedEntry = null;

            var entries = Clipboard.GetEntries();

            var selectedIndex = 0;
            foreach (var entry in entries)
            {
                Entries.Add(new EntryViewModel(entry));

                if (entry.Format == selectedFormat)
                {
                    selectedIndex = Entries.Count - 1;
                }
            }

            if (Entries.Count > selectedIndex)
            {
                SelectedEntry = Entries[selectedIndex];
            }

            SequenceNumber = String.Format("Sequence number: {0}", Clipboard.GetSequenceNumber());
            this.OnPropertyChanged(() => SequenceNumber);
        }

        public void UpdateHexData()
        {
            HexLines.Clear();

            if (null == _selectedEntry)
            {
                return;
            }

            var data = Clipboard.GetData(_selectedEntry.Format);

            if (0 == data.Length)
            {
                return;
            }

            var offset = 0;

            var remaining = data.Length;
            while (remaining > 0)
            {
                var line = new Byte[Math.Min(remaining, 16)];

                Array.Copy(data, offset, line, 0, line.Length);

                HexLines.Add(new HexLineViewModel(offset, line));

                offset += 16;
                remaining -= 16;
            }
        }
    }
}
