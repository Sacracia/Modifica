using ModificaWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        private object frameContent;
        private Button lastBtn;

        public object FrameContent
        {
            get => frameContent;
            set
            {
                frameContent = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            AppNotifier.Setup();
            InitializeComponent();
            AppLogic.Instance.mainPage = this;
            AppLogic.Instance.NavigateTo<WelcomePage>();
        }

        private void NavigateClick(object sender, RoutedEventArgs e)
        {
            var elem = (Button)sender;
            AppLogic.Instance.NavigateTo(elem.Tag?.GetType());
            if (lastBtn != null)
                lastBtn.SetValue(ButtonProperties.SelectedProperty, false);
            else
                HomeBtn.SetValue(ButtonProperties.SelectedProperty, false);
            lastBtn = elem;
            lastBtn.SetValue(ButtonProperties.SelectedProperty, true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
