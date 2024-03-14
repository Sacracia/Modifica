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

        public void AddCard(string name, int options, string desc)
        {
            if (pos < 8)
            {
                Button b = new Button();
                b.VerticalAlignment = VerticalAlignment.Bottom;
                b.HorizontalAlignment = HorizontalAlignment.Right;
                b.Template = (ControlTemplate)App.Current.Resources["CustomModCard"];
                b.Content = name;
                Grid.SetColumn(b, pos % 3);
                Grid.SetRow(b, pos / 3);
                CustomModsGrid.Children.Add(b);
                pos += 1;
                Grid.SetRow(AddBtn, pos / 3);
                Grid.SetColumn(AddBtn, pos % 3);
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pos < 8)
            {
                AppLogic.Instance.MainNavigateTo<SettingsPage>();
            }
        }
    }
}
