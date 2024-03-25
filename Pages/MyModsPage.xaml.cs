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
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
