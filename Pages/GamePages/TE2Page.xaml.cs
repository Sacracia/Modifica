using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModificaWPF.Pages.GamePages
{
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
                    await LoaderLogic.Instance.Load(LoaderLogic.Instance.te2Config);
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