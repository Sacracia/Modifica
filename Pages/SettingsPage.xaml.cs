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
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page, INotifyPropertyChanged
    {
        private string gameName;
        private string options;
        private string description;
        private string processName;
        private string modPath;
        private string nspace;
        private string klass;
        private string method;
        private int harmonyV;

        public string GameName
        {
            get { return gameName; }
            set { gameName = value; OnPropertyChanged("GameName"); }
        }

        public string Options
        {
            get { return options; }
            set { options = value; OnPropertyChanged("Options"); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        public string ProcessName
        {
            get { return processName; }
            set { processName = value; OnPropertyChanged("ProcessName"); }
        }

        public string ModPath
        {
            get { return modPath; }
            set { modPath = value; OnPropertyChanged("ModPath"); }
        }

        public string Namespace
        {
            get { return nspace; }
            set { nspace = value; OnPropertyChanged("Namespace"); }
        }

        public string Class
        {
            get { return klass; }
            set { klass = value; OnPropertyChanged("Class"); }
        }

        public string Methode
        {
            get { return method; }
            set { method = value; OnPropertyChanged("Methode"); }
        }

        public int HarmonyV
        {
            get { return harmonyV; }
            set { harmonyV = value; OnPropertyChanged("HarmonyV"); }
        }

        public UserModConfig Config { get; set; }

        public int Pos { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
        }

        public void SetConfig(UserModConfig cfg)
        {
            Config = cfg;
            GameName = cfg.Naming;
            Options = cfg.OptionsNumber.ToString();
            Description = cfg.Description;
            ProcessName = cfg.ProcName;
            ModPath = cfg.ModPath;
            Namespace = cfg.Nspace;
            Class = cfg.Klass;
            Methode = cfg.Method;
            HarmonyV = HarmonyVersion.Items.IndexOf(HarmonyVersion.Items.OfType<ComboBoxItem>().FirstOrDefault(x => x.Content.ToString() == cfg.HarmonyVersion));
            Pos = cfg.PosInArr;
        }

        private void NavigateClick(object sender, RoutedEventArgs e)
        {
            AppLogic.Instance.MainNavigateTo<MainPage>();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (LoaderLogic.Instance.AreArgsCorrect(GameName, Options, Description, ProcessName, ModPath, Namespace, Class, Methode))
            {
                Config.Naming = GameName;
                Config.OptionsNumber = int.Parse(Options);
                Config.Description = Description;
                Config.ProcName = ProcessName;
                Config.ModPath = ModPath;
                Config.Nspace = Namespace;
                Config.Klass = Class;
                Config.Method = Methode;
                Config.HarmonyVersion = (HarmonyVersion.Items[HarmonyV] as ComboBoxItem).Content.ToString();
                AppLogic.Instance.MainNavigateTo<MainPage>();
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            MyModsPage page = (MyModsPage)App.Current.FindResource("MyModsPage");
            if (page != null)
            {
                page.CustomElements.Children.RemoveAt(Config.CardPos);
                LoaderLogic.Instance.userConfigs[Config.PosInArr] = null;
                page.counter--;
                AppLogic.Instance.MainNavigateTo<MainPage>();
            }
        }

        private void ChoosePathClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "(.dll)|*.dll";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                ModPath = dialog.FileName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
