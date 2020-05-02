using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LibrarySystem.Converters
{
    /// <summary>
    /// Konvertar en string till maskade karaktärer
    /// </summary>
    public class StringToPasswordConverter : IValueConverter
    {
        /// <summary>
        /// Sparar och håller reda på den 'rena' stringen för konvertering tillbaka
        /// </summary>
        private static string StoredCleanPassword;

        /// <summary>
        /// String till maskad
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string cleanPassword = (string)value;
            string maskedPassword = "";

            foreach(char chr in cleanPassword)
            {
                maskedPassword += '*';
            }

            StoredCleanPassword = cleanPassword;

            return maskedPassword;
        }

        /// <summary>
        /// Maskad till ren string
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string maskedPassword = (string)value;
            string cleanPassword = "";
            // Indexer for the stored password
            int j = 0;

            foreach (char chr in maskedPassword)
            {
                //todo; check if password containing an '*' works
                if (chr == '*')
                    cleanPassword += StoredCleanPassword[j];
                else
                    cleanPassword += chr;

                j++;
            }
            return cleanPassword;
        }
    }
}
