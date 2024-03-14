using System.Windows;
using System.Windows.Controls;

namespace ModificaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
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
                if (myModsPage.pos < 8)
                {
                    if (!LoaderLogic.Instance.AddConfig(Naming.Text, OptsNum.Text, Desc.Text, ProcName.Text, FilePath.Text, NSpace.Text, Klass.Text, Method.Text, HarmonyVersion.Text))
                    {
                        AppNotifier.Error("Failed to add mod");
                    }
                }
            }
            AppLogic.Instance.MainNavigateTo<MainPage>();
        }
    }
}
