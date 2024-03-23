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
                b.Loaded += (s, e) =>
                {
                    TextBlock header = FindChild<TextBlock>(b, "Header");
                    TextBlock descr = FindChild<TextBlock>(b, "Description");
                    TextBlock opts = FindChild<TextBlock>(b, "Options");
                    if (header != null)
                    {
                        header.Text = name;
                    }
                    if (descr != null)
                    {
                        descr.Text = $"Description: {desc}";
                    }
                    if (opts != null)
                    {
                        opts.Text = $"Options: {options}";
                    }
                };
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
