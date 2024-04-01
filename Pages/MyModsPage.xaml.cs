using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace ModificaWPF.Pages
{
    public partial class MyModsPage : Page
    {
        public int counter = 0;
        public MyModsPage()
        {
            InitializeComponent();
        }

        public void AddCard(UserModConfig cfg)
        {
            int pos = cfg.PosInArr;
            if (pos < 8)
            {
                Button b = new Button();
                b.Template = (ControlTemplate)App.Current.Resources["CustomModCard"];
                b.Margin = new Thickness(24, 24, 0, 0);
                b.Loaded += (s, e) =>
                {
                    TextBlock header = FindChild<TextBlock>(b, "Header");
                    TextBlock descr = FindChild<TextBlock>(b, "Description");
                    TextBlock opts = FindChild<TextBlock>(b, "Options");
                    Button settingsBtn = FindChild<Button>(b, "SettingsButton");
                    settingsBtn.Tag = cfg;
                    Button loadBtn = FindChild<Button>(b, "LoadModBtn");
                    loadBtn.Tag = cfg;

                    Binding binding = new Binding();
                    binding.Source = cfg;
                    binding.Path = new PropertyPath("Naming");
                    binding.Mode = BindingMode.TwoWay;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    header.SetBinding(TextBlock.TextProperty, binding);

                    Binding bindingOpts = new Binding();
                    bindingOpts.Source = cfg;
                    bindingOpts.Path = new PropertyPath("OptionsNumber");
                    bindingOpts.Mode = BindingMode.TwoWay;
                    bindingOpts.StringFormat = "Options: {0}";
                    bindingOpts.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    opts.SetBinding(TextBlock.TextProperty, bindingOpts);

                    Binding bindingDesc = new Binding();
                    bindingDesc.Source = cfg;
                    bindingDesc.Path = new PropertyPath("Description");
                    bindingDesc.Mode = BindingMode.TwoWay;
                    bindingDesc.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    descr.SetBinding(TextBlock.TextProperty, bindingDesc);
                };
                cfg.CardPos = counter;
                CustomElements.Children.Insert(counter, b);
                counter += 1;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (counter < 8)
            {
                AppLogic.Instance.MainNavigateTo<NewModPage>();
            }
        }

        public T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) 
                return null;
            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);

                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
