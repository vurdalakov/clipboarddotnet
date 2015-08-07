namespace Vurdalakov.ClipboardDotNet
{
    using System;

    public class MainViewModel : ViewModelBase
    {
        public ThreadSafeObservableCollection<EntryViewModel> Entries { get; private set; }

        public EntryViewModel SelectedEntry { get; set; }

        public MainViewModel()
        {
            Entries = new ThreadSafeObservableCollection<EntryViewModel>();
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
    }
}
