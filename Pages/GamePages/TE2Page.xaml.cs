using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModificaWPF.Pages.GamePages
{
    /// <summary>
    /// Логика взаимодействия для TE2Page.xaml
    /// </summary>
    public partial class TE2Page : Page
    {
        public TE2Page()
        {
            InitializeComponent();
        }

        private void NavigateClick(object sender, RoutedEventArgs e)
        {
            AppLogic.Instance.NavigateTo(typeof(GamesPage));
        }

        private async void LoadClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                await Func(btn);
            }
        }

        private async Task Func(Button btn)
        {
            try
            {
                await App.Current.Dispatcher.Invoke(async () =>
                {
                    btn.IsEnabled = false;
                    btn.Opacity = 0.7;
                    //await Func2();
                    await LoaderLogic.Instance.Load(LoaderLogic.Instance.etgConfig);
                    //await Task.Delay(1000);
                    btn.IsEnabled = true;
                    btn.Opacity = 1;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
