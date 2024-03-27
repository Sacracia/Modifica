using ModificaWPF.Pages;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
            CheckVersion();
        }

        private async void CheckVersion()
        {
            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Version v = await Downloader.GetLatestVersion();
            if (v != null && v > currentVersion)
            {
                AppNotifier.Info($"Version {v} is available!");
            }
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
