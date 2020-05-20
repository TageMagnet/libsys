using Library;
using LibrarySystem.Models;
using LibrarySystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;

namespace LibrarySystem.ViewModels
{
    public class LibrarianViewModel : BaseViewModel
    {
        private MemberRepository memberRepo = new MemberRepository();
        public bool TestCheck { get; set; } = true;
        public static UserControl CurrentBackEndPage { get; set; }

        // Private holder
        private int tabControlSelectedIndex { get; set; } = 0;

        /// <summary>
        /// Trigggers when tab item is changed
        /// </summary>
        public int TabControlSelectedIndex
        {
            get
            {
                return tabControlSelectedIndex;
            }
            set
            {
                tabControlSelectedIndex = value;
                //OnPropertyChanged("TabControlSelectedIndex");

                switch ((int)value)
                {
                    case 0:
                        #region BookView
                        CurrentBackEndPage.Content = new BookView();
                        break;
                    #endregion
                    case 1:
                        #region EventView
                        CurrentBackEndPage.Content = new EventView();
                        break;
                    #endregion
                    case 2:
                        #region MemberView
                        CurrentBackEndPage.Content = new MemberView();
                        // Ladda members när tab control bytts till rätt sida
                        break;
                    #endregion
                    case 3:
                        #region AuthorView
                        CurrentBackEndPage.Content = new AuthorView();
                        break;
                        #endregion
                }
            }
        }

        public LibrarianViewModel()
        {
            CurrentBackEndPage = new BookView();
        }
    }
}
