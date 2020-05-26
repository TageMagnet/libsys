using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace LibrarySystem.Converters
{
    /// <summary>
    /// Transform certain types of string to color
    /// </summary>
    public class StringToColor : IValueConverter
    {
        private static string storedValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value.ToString().ToLower();
            storedValue = val;

            switch (val)
            {
                case "ok":
                    return new SolidColorBrush(Colors.Green);
                case "försenad":
                    return new SolidColorBrush(Colors.Red);
                case "kanske":
                    return new SolidColorBrush(Colors.Orange);
            }

            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(Colors.Gray);
            //return storedValue;
        }
    }
}
