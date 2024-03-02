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
            ApplicationLogic.Instance.NavigateTo(typeof(GamesPage));
        }
    }
}
