using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OOP_CourseProject
{
    /// <summary>
    /// This class is used to convert enum values to their descriptions for display in the UI.
    /// </summary>
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var description = enumValue
                    .GetType()
                    .GetField(enumValue.ToString())
                    ?.GetCustomAttribute<DescriptionAttribute>()?.Description;

                return description ?? enumValue.ToString();
            }

            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This converter is only for display, not converting back
            throw new NotImplementedException();
        }
    }
}
