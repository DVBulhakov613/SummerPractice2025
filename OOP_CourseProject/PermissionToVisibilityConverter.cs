using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace OOP_CourseProject
{
    /// <summary>
    /// This class is used to convert a permission ID to a visibility value.
    /// </summary>
    public class PermissionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string permissionIdStr && int.TryParse(permissionIdStr, out var permissionId))
            {
                return App.CurrentEmployee.HasPermission(permissionId) ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
