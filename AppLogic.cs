using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModificaWPF
{
    internal class AppLogic : IDisposable
    {
        private static AppLogic s_instance;

        private AppLogic() { }

        public static AppLogic Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new AppLogic();
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

        public void Dispose()
        {
            AppNotifier.Shutdown();
        }
    }
}
