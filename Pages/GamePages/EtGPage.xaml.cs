using System;
using System.Windows;
using System.Windows.Controls;

namespace ModificaWPF.Pages.GamePages
{
    /// <summary>
    /// Логика взаимодействия для EtGPage.xaml
    /// </summary>
    public partial class EtGPage : Page
    {
        public EtGPage()
        {
            InitializeComponent();
        }

        private void NavigateClick(object sender, RoutedEventArgs e)
        {
           AppLogic.Instance.NavigateTo(typeof(GamesPage));
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {

            Dispatcher?.BeginInvoke(new Action(() =>
            {
                try
                {
                    Button btn = (Button)sender;
                    btn.IsEnabled = false;
                    btn.Opacity = 0.7;
                    LoaderLogic.Instance.Load(LoaderLogic.Instance.etgConfig);
                    btn.IsEnabled = true;
                    btn.Opacity = 1;
                }
                catch
                {

                }
            }));
        }
    }
}
