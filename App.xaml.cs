using ModificaWPF.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModificaWPF
{
    public partial class App : Application
    {
        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button settingsBtn)
            {
                SettingsPage page = (SettingsPage)Application.Current.FindResource("SettingsPage");
                page.SetConfig((UserModConfig)settingsBtn.Tag);
                AppLogic.Instance.MainNavigateTo<SettingsPage>();
            }
        }

        private async void LoadModClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                try
                {
                    btn.IsEnabled = false;
                    btn.Opacity = 0.7;
                    var task = await LoaderLogic.Instance.LoadCustomMod(btn.Tag as UserModConfig);
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
