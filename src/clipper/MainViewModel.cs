namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Win32;

    public class MainViewModel : ViewModelBase, IHexLineDataSource
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

                    UpdateFormat();
                }
            }
        }

        public ThreadSafeObservableCollection<HexLineViewModel> HexLines { get; private set; }
        public String Text { get; private set; }

        public String SequenceNumber { get; private set; }

        private ClipboardListener _clipboardListener = new ClipboardListener();

        public MainViewModel()
        {
            Entries = new ThreadSafeObservableCollection<EntryViewModel>();
            HexLines = new ThreadSafeObservableCollection<HexLineViewModel>();

            IsAutoFormat = true;

            _clipboardListener.ClipboardUpdated += (s, e) => Refresh();

            this.SaveCommand = new CommandBase(OnSaveCommand);
            this.RestoreCommand = new CommandBase(OnRestoreCommand);
            this.SaveFormatCommand = new CommandBase(OnSaveFormatCommand);
            this.ExitCommand = new CommandBase(OnExitCommand);
            this.CopyCommand = new CommandBase<String>(OnCopyCommand);
            this.EmptyCommand = new CommandBase(OnEmptyCommand);
            this.AsAutoCommand = new CommandBase(OnAsAutoCommand);
            this.AsBinaryCommand = new CommandBase(OnAsBinaryCommand);
            this.AsTextCommand = new CommandBase(OnAsTextCommand);
            this.RefreshCommand = new CommandBase(OnRefreshCommand);
            this.AboutCommand = new CommandBase(OnAboutCommand);
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

        public Boolean IsAutoFormat { get; private set; }
        public Boolean IsTextFormat { get; private set; }
        public Boolean IsBinaryFormat { get; private set; }

        private void UpdateFormat()
        {
            if (IsAutoFormat)
            {
                IsTextFormat = (_selectedEntry != null) && ClipboardText.IsTextFormat(_selectedEntry.Format);

                IsBinaryFormat = !IsTextFormat;
                this.OnPropertyChanged(() => IsBinaryFormat);
            }

            this.OnPropertyChanged(() => IsAutoFormat);
            this.OnPropertyChanged(() => IsBinaryFormat);
            this.OnPropertyChanged(() => IsTextFormat);

            UpdateData();
        }

        private Byte[] _data;

        private void UpdateData()
        {
            HexLines.Clear();
            Text = "";

            if (null == _selectedEntry)
            {
                return;
            }

            _data = Clipboard.GetData(_selectedEntry.Format);

            if (0 == _data.Length)
            {
                return;
            }

            if (IsTextFormat && ClipboardText.IsTextFormat(_selectedEntry.Format))
            {
                var data = Clipboard.GetData(_selectedEntry.Format);
                Text = ClipboardText.ExtractText(_selectedEntry.Format, data);
            }
            else if (IsBinaryFormat)
            {
                var offset = 0;

                var remaining = _data.Length;
                while (remaining > 0)
                {
                    HexLines.Add(new HexLineViewModel(offset, this));

                    offset += 16;
                    remaining -= 16;
                }
            }

            OnPropertyChanged(() => Text);
        }

        public Byte[] GetData(Int32 offset)
        {
            var line = new Byte[Math.Min(_data.Length - offset, 16)];
            Array.Copy(_data, offset, line, 0, line.Length);

            return line;
        }

        public ICommand SaveCommand { get; private set; }
        public void OnSaveCommand()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".clp";
            saveFileDialog.Filter = "Clipboard File|*.clp";

            if (true == saveFileDialog.ShowDialog())
            {
                var fileName = saveFileDialog.FileName;
                ClipboardFile.Save(fileName);
            }
        }

        public ICommand RestoreCommand { get; private set; }
        public void OnRestoreCommand()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".clp";
            openFileDialog.Filter = "Clipboard File|*.clp";

            if (true == openFileDialog.ShowDialog())
            {
                var fileName = openFileDialog.FileName;
                ClipboardFile.Restore(fileName);
            }
        }

        public ICommand SaveFormatCommand { get; private set; }
        public void OnSaveFormatCommand()
        {
            if (null == _selectedEntry)
            {
                return;
            }

            var isText = ClipboardText.IsTextFormat(_selectedEntry.Format);

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = isText ? ".txt" : ".bin";
            saveFileDialog.Filter = isText ? "Text File|*.txt" : "Binary File|*.bin";

            if (true == saveFileDialog.ShowDialog())
            {
                var fileName = saveFileDialog.FileName;

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (var binaryWriter = new BinaryWriter(fileStream))
                    {
                        var data = Clipboard.GetData(_selectedEntry.Format);

                        var count = data.Length;
                        if (isText)
                        {
                            while ((count > 0) && ('\0' == data[count - 1]))
                            {
                                count--;
                            }
                        }

                        binaryWriter.Write(data, 0, count);
                    }
                }
            }
        }

        public ICommand ExitCommand { get; private set; }
        public void OnExitCommand()
        {
            Application.Current.Shutdown();
        }

        public ICommand CopyCommand { get; private set; }
        public void OnCopyCommand(String source)
        {
            if (null == _selectedEntry)
            {
                return;
            }

            switch (source)
            {
                case "1":
                    Clipboard.SetText(_selectedEntry.Format.ToString());
                    break;
                case "2":
                    Clipboard.SetText(_selectedEntry.DataSize.ToString());
                    break;
                case "3":
                    Clipboard.SetText(_selectedEntry.Name);
                    break;
            }
        }

        public ICommand EmptyCommand { get; private set; }
        public void OnEmptyCommand()
        {
            Clipboard.Empty();
        }

        public ICommand AsAutoCommand { get; private set; }
        public void OnAsAutoCommand()
        {
            IsAutoFormat = true;
            UpdateFormat();
        }

        public ICommand AsBinaryCommand { get; private set; }
        public void OnAsBinaryCommand()
        {
            IsAutoFormat = false;
            IsBinaryFormat = true;
            IsTextFormat = false;
            UpdateFormat();
        }

        public ICommand AsTextCommand { get; private set; }
        public void OnAsTextCommand()
        {
            IsAutoFormat = false;
            IsBinaryFormat = false;
            IsTextFormat = true;
            UpdateFormat();
        }

        public ICommand RefreshCommand { get; private set; }
        public void OnRefreshCommand()
        {
            Refresh();
        }

        public ICommand AboutCommand { get; private set; }
        public void OnAboutCommand()
        {
            MessageBox.Show("Clipper 2.0\n\nCopyright @ 1997, 2015 Vurdalakov\n\nhttp://www.vurdalakov.net/\nvurdalakov@gmail.com", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
