using LibrarySystem.Datamodel;
using LibrarySystem.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LibrarySystem.Converters
{
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Admin:
                    return new AdminView();
                case ApplicationPage.Customer:
                    return new CustomerView();
                case ApplicationPage.Home:
                    return new HomeView();
                case ApplicationPage.Librarian:
                    return new LibrarianView();
                case ApplicationPage.Login:
                    return new LoginView();
                default:
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
