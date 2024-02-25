using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModificaWPF
{
    internal class ApplicationLogic
    {
        private static ApplicationLogic s_instance;

        public static ApplicationLogic Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new ApplicationLogic();
                return s_instance;
            }
        }

        public void NavigateTo(Type page)
        {
            if (Application.Current.MainWindow is MainWindow window)
            {
                window.FrameContent = Application.Current.FindResource(page.Name);
            }
        }

        public void NavigateTo<TPage>() where TPage : Page => NavigateTo(typeof(TPage));
    }
}
