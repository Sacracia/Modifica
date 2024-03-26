using ModificaWPF.Pages;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ModificaWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private object frameContent;

        public object FrameContent
        {
            get => frameContent;
            set
            {
                frameContent = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            AppNotifier.Setup();
            LoaderLogic.Instance.Deserialize();
            InitializeComponent();
            AppLogic.Instance.MainNavigateTo<MainPage>();
        }

        private void CloseClick(Object sender, RoutedEventArgs e)
        {
            LoaderLogic.Instance.Serialize();
            Close();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
