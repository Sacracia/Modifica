using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModificaWPF.Models
{
    public static class ButtonProperties
    {
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.RegisterAttached("Selected", typeof(bool), typeof(ButtonProperties), new UIPropertyMetadata(default(bool)));

        public static readonly DependencyProperty Text =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(ButtonProperties), new UIPropertyMetadata(default(string)));

        public static bool GetSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectedProperty);
        }

        public static void SetSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectedProperty, value);
        }

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(Text);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(Text, value);
        }

        public static readonly DependencyProperty HyperLink =
            DependencyProperty.RegisterAttached("HyperLink", typeof(string), typeof(ButtonProperties), new UIPropertyMetadata(default(string)));

        public static string GetHyperLink(DependencyObject obj)
        {
            return (string)obj.GetValue(HyperLink);
        }

        public static void SetHyperLink(DependencyObject obj, string value)
        {
            obj.SetValue(HyperLink, value);
        }
    }
}
