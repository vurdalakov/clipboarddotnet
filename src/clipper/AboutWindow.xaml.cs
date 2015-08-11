namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Windows;

    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
        }
    }
}
