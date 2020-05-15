using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LibrarySystem.Converters
{
    public class IntToEmptyConverter : IValueConverter
    {
        private static int storedValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            int x = (int)value;
            storedValue = x;
            if (x > 0)
                return x;

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int x = System.Convert.ToInt32(value);
            return x;
        }
    }
}
