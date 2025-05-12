using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OOP_CourseProject.Controls.Helpers
{
    public static class TextBoxHelper
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits)
            );

        public static void SetPlaceholder(DependencyObject element, string value)
        {
            element.SetValue(PlaceholderProperty, value);
        }

        public static string GetPlaceholder(DependencyObject element)
        {
            return (string)element.GetValue(PlaceholderProperty);
        }
    }

}
