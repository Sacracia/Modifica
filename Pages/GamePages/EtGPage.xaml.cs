using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ToastNotifications.Core;

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
                try
                {
                    btn.IsEnabled = false;
                    btn.Opacity = 0.7;
                    var task = await LoaderLogic.Instance.Load(LoaderLogic.Instance.EtgConfig);
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