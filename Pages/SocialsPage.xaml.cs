using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ModificaWPF.Pages
{
    public partial class SocialsPage : Page
    {
        public static readonly Uri GITHUB_LINK = new Uri("https://github.com/Sacracia/Modifica");
        public static readonly Uri YOUTUBE_LINK = new Uri("https://www.youtube.com/@Modifica-in");
        public static readonly Uri DISCORD_LINK = new Uri("https://discord.gg/E8B4X9s");        

        public SocialsPage()
        {
            InitializeComponent();
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
