using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LibrarySystem.Converters
{
    /// <summary>
    /// Force convert input string into int
    /// </summary>
    public class StringToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int n;
            bool isNumeric = int.TryParse(value.ToString(), out n);
            return isNumeric ? n : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int n;
            bool isNumeric = int.TryParse(value.ToString(), out n);
            return isNumeric ? n : 1;
        }
    }
}
