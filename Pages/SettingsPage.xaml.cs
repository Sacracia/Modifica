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
                    Border b = new Border();
                    b.VerticalAlignment = VerticalAlignment.Bottom;
                    b.HorizontalAlignment = HorizontalAlignment.Right;
                    b.Width = 242;
                    b.Height = 154;
                    b.Background = (Brush)(new BrushConverter().ConvertFrom("#1A2026"));
                    b.CornerRadius = new CornerRadius(10);
                    Grid.SetColumn(b, myModsPage.pos % 3);
                    Grid.SetRow(b, myModsPage.pos / 3);
                    myModsPage.CustomModsGrid.Children.Add(b);
                    myModsPage.pos += 1;
                    Grid.SetRow(myModsPage.AddBtn, myModsPage.pos / 3);
                    Grid.SetColumn(myModsPage.AddBtn, myModsPage.pos % 3);
                }
            }
            AppLogic.Instance.MainNavigateTo<MainPage>();
        }
    }
}
