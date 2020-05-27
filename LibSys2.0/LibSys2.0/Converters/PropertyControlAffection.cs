using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LibrarySystem.Converters
{
    /// <summary>
    /// Change property depending on incoming property
    /// </summary>
    public class PropertyControlAffection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Visible";

            string str = value.ToString().ToLower();

            if (str.Contains("ebook"))
                return "Visible";

            return "Hidden";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
