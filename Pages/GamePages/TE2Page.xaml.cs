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
                try
                {
                    btn.IsEnabled = false;
                    btn.Opacity = 0.7;
                    var task = await LoaderLogic.Instance.Load(LoaderLogic.Instance.te2Config);
                    switch (task.Item1)
                    {
                        case 0:
                            AppNotifier.Success(task.Item2);
                            break;
                        case 1:
                            AppNotifier.Error(task.Item2);
                            break;
                        case 2:
                            AppNotifier.Warning(task.Item2);
                            break;
                    }
                    btn.IsEnabled = true;
                    btn.Opacity = 1;
                }
                catch (Exception ex)
                {
                    btn.IsEnabled = true;
                    btn.Opacity = 1;
                    AppNotifier.Error(ex.Message);
                }
            }
        }
    }
}