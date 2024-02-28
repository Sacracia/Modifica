using ModificaWPF.Models;
using ModificaWPF.Pages;
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

namespace ModificaWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
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

        public MainWindow()
        {
            InitializeComponent();
            ApplicationLogic.Instance.NavigateTo<WelcomePage>();
        }

        private void CloseClick(Object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void NavigateClick(object sender, RoutedEventArgs e)
        {
            if (lastBtn != null)
                lastBtn.SetValue(ButtonProperties.SelectedProperty, false);
            else
                HomeBtn.SetValue(ButtonProperties.SelectedProperty, false);
            lastBtn = (Button)sender;
            lastBtn.SetValue(ButtonProperties.SelectedProperty, true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
