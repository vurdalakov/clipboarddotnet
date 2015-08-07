namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Windows;

    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += this.OnMainWindowLoaded;
            this.Closing += OnMainWindowClosing;
        }

        private void OnMainWindowLoaded(Object sender, RoutedEventArgs e)
        {
            this.DataContext = this.mainViewModel;

            this.mainViewModel.OnLoaded();
        }

        private void OnMainWindowClosing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.mainViewModel.OnClosing();
        }
    }
}
