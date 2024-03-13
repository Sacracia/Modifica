using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModificaWPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для MyModsPage.xaml
    /// </summary>
    public partial class MyModsPage : Page
    {
        public int pos = 0;
        public MyModsPage()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pos < 8)
            {
                /*Border b = new Border();
                b.VerticalAlignment = VerticalAlignment.Bottom;
                b.HorizontalAlignment = HorizontalAlignment.Right;
                b.Width = 242;
                b.Height = 154;
                b.Background = (Brush)(new BrushConverter().ConvertFrom("#1A2026"));
                b.CornerRadius = new CornerRadius(10);
                Grid.SetColumn(b, pos % 3);
                Grid.SetRow(b, pos / 3);
                CustomModsGrid.Children.Add(b);
                pos += 1;
                Grid.SetRow(AddBtn, pos / 3);
                Grid.SetColumn(AddBtn, pos % 3);*/
                AppLogic.Instance.MainNavigateTo<SettingsPage>();
            }
        }
    }
}
