using System.Windows;
using System.Windows.Controls;

namespace ModificaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class NewModPage : Page
    {
        public NewModPage()
        {
            InitializeComponent();
        }

        private void NavigateClick(object sender, RoutedEventArgs e)
        {
            AppLogic.Instance.MainNavigateTo<MainPage>();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.FindResource("MyModsPage") is MyModsPage myModsPage)
            {
                if (myModsPage.counter < 8)
                {
                    if (!LoaderLogic.Instance.AddConfig(Naming.Text, OptsNum.Text, Desc.Text, ProcName.Text, FilePath.Text, NSpace.Text, Klass.Text, Method.Text, HarmonyVersion.Text))
                    {
                        AppNotifier.Error("Failed to add mod");
                    }
                    else
                    {
                        AppLogic.Instance.MainNavigateTo<MainPage>();
                    }
                }
            }
        }

        private void ChoosePathClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "(.dll)|*.dll";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                FilePath.Text = dialog.FileName;
            }
        }
    }
}
