﻿namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Text;

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

        public String HexData { get; private set; }

        public MainViewModel()
        {
            Entries = new ThreadSafeObservableCollection<EntryViewModel>();

            _clipboardListener.ClipboardUpdate += OnClipboardUpdate;
        }

        private void OnClipboardUpdate(Object sender, EventArgs e)
        {
            Refresh();
        }

        private ClipboardListener _clipboardListener = new ClipboardListener();

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
            Entries.Clear();

            var entries = Clipboard.GetEntries();

            foreach (var entry in entries)
            {
                Entries.Add(new EntryViewModel(entry));
            }

            if (Entries.Count > 0)
            {
                SelectedEntry = Entries[0];
            }
        }

        public void UpdateHexData()
        {
            var data = Clipboard.GetData(_selectedEntry.Format);

            var chars = new Char[16];

            var hexData = new StringBuilder();

            var pos = 0;
            for (;;)
            {
                var stringBuilder = new StringBuilder(128);
                stringBuilder.AppendFormat("{0:X08}: ", pos);

                for (var i = 0; i < 16; i++)
                {
                    if (8 == i)
                    {
                        stringBuilder.Append("| ");
                    }

                    if (pos < data.Length)
                    {
                        stringBuilder.AppendFormat("{0:X02} ", data[pos]);
                        chars[i] = data[pos] > 31 ? (char)data[pos] : ' ';
                        pos++;
                    }
                    else
                    {
                        stringBuilder.Append("   ");
                        chars[i] = ' ';
                    }
                }

                stringBuilder.Append(' ');
                stringBuilder.Append(chars);
                stringBuilder.Append(Environment.NewLine);

                hexData.Append(stringBuilder);

                if (pos >= data.Length)
                {
                    break;
                }
            }

            HexData = hexData.ToString();
            this.OnPropertyChanged(() => HexData);
        }
    }
}
