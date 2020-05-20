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
        public Member NewMember { get; set; }
        public ObservableCollection<Member> Members { get; set; } = new ObservableCollection<Member>();

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
                        CurrentBackEndPage.Content = new BookView();
                        NewMember = new Member();
                        // Ladda members när tab control bytts till rätt sida
                        _ = LoadMembers();
                        break;
                    case 1:
                        CurrentBackEndPage.Content = new EventView();
                        NewMember = new Member();
                        // Ladda members när tab control bytts till rätt sida
                        _ = LoadMembers();
                        break;
                    case 3:
                        CurrentBackEndPage.Content = new AuthorView();
                        NewMember = new Member();
                        // Ladda members när tab control bytts till rätt sida
                        _ = LoadMembers();
                        break;
                    case 4:
                        NewMember = new Member();
                        CurrentBackEndPage.Content = new MemberView();
                        // Ladda members när tab control bytts till rätt sida
                        _ = LoadMembers();
                        break;
                }
            }
        }

        public LibrarianViewModel()
        {
            CurrentBackEndPage = new BookView();
        }

        public async Task LoadMembers()
        {
            Members.Clear();

            foreach (Member member in await memberRepo.ReadAllActive())
            {
                member.ref_member_role_id--;
                Members.Add(member);
            }
        }
    }
}
