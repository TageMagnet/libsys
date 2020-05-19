using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LibrarySystem.Converters
{
    public class EmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                str = (string)parameter;

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
