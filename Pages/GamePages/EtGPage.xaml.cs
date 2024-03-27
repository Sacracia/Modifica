using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ModificaWPF.Pages.GamePages
{
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
                    await LoaderLogic.Instance.Load(LoaderLogic.Instance.etgConfig);
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