using System.Windows;

namespace ModificaWPF.Models
{
    public static class ButtonProperties
    {
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.RegisterAttached("Selected", typeof(bool), typeof(ButtonProperties), new UIPropertyMetadata(default(bool)));

        public static bool GetSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectedProperty);
        }

        public static void SetSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectedProperty, value);
        }
    }
}
