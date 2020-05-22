using Library;
using LibrarySystem.Models;
using LibrarySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBox = System.Windows.MessageBox;

namespace LibrarySystem
{
    class MemberViewModel : BaseViewModel
    {
        #region Commands
        public RelayCommand AddNewMember { get; set; }
        public RelayCommandWithParameters DeleteMemberCommand { get; set; }
        public RelayCommandWithParameters UpdateMemberCommand { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<string> AvailableRoles { get; set; } = new ObservableCollection<string>() { "admin", "librarian", "user", "guest" };
        public int SelectedRoleIndex { get; set; }
        public MemberRepository memberRepo = new MemberRepository();
        public Member SelectedMember { get; set; } = new Member();
        public ObservableCollection<Member> Members { get; set; } = new ObservableCollection<Member>();
        public Member NewMember { get; set; }
        #endregion
        public MemberViewModel()
        {
            NewMember = new Member();
            AddNewMember = new RelayCommand(async () => await AddNewMemberCommand());
            DeleteMemberCommand = new RelayCommandWithParameters(async (param) => await DeleteMemberCommandMethod(param));
            UpdateMemberCommand = new RelayCommandWithParameters(async (param) => await UpdateMemberCommandMethod((Member)param));
            InitLoad();
        }
        public async Task UpdateMemberCommandMethod(Member member)
        {
            Member membercheck = (await memberRepo.SearchByColumn("member_id", member.member_id.ToString())).First();
            // Fix since MYSQL starts index at 1

            member.ref_member_role_id++;
            
            //Check if the logged in user have permission to update
            if (membercheck.ref_member_role_id < Globals.LoggedInUser.ref_member_role_id)
            {
                MessageBox.Show("Du har inte behörighet att uppdatera en Admin");
                return;
            }
            if (member.ref_member_role_id < Globals.LoggedInUser.ref_member_role_id)
            {
                MessageBox.Show("Du kan inte uppdatera till en högre privilegie än vad du redan har");
                return;
            }

            SelectedRoleIndex = AvailableRoles.IndexOf(member.role);
            await memberRepo.Update(member);

            //Update the logged in user if changes are to the logged in user
            if (member.member_id == Globals.LoggedInUser.member_id)
            {
                await Globals.UpdateGlobalUser();
            }
            await LoadMembers();

        }
        public async void InitLoad() => await LoadMembers();
        public async Task LoadMembers()
        {
            Members.Clear();

            foreach (Member member in await memberRepo.ReadAllActive())
            {
                member.ref_member_role_id--;
                Members.Add(member);
            }
        }

        public async Task DeleteMemberCommandMethod(object obj)
        {
            Member member = (Member)obj;
            member.is_active = 0;
            await UpdateMemberCommandMethod(member);
            await LoadMembers();
        }

        /// <summary>
        /// Reset input lines
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async Task ClearMemberLines(string sender)
        {
            //Clear Members
            NewMember.email = "";
            NewMember.nickname = "";
            NewMember.pwd = "";
            NewMember.role = null;
            this.OnPropertyChanged(nameof(NewMember));

        }
        /// <summary>
        /// Adds new member
        /// </summary>
        /// <returns></returns>
        public async Task AddNewMemberCommand()
        {
            if (NewMember.email == null)
            {
                MessageBox.Show("Lägg till E-Post");
                return;
            }

            if (NewMember.nickname == null)
            {
                MessageBox.Show("Lägg till smeknamn");
                return;
            }
            if (NewMember.pwd == null)
            {
                MessageBox.Show("Lägg till lösenord");
                return;
            }
            if (NewMember.role == null)
            {
                MessageBox.Show("Lägg till roll");
                return;
            }
            List<Member> memberlist = await memberRepo.SearchByColumn("email", NewMember.email);
            if (memberlist.Count > 0)
            {
                MessageBox.Show("Denna email finns redan!");
                return;
            }
            if (NewMember.role == "admin" && Globals.LoggedInUser.ref_member_role_id != 1)
            {
                MessageBox.Show("Du har inte behörighet att lägga till en Admin");
                return;
            }
            // Timestamp, since now is creation date
            NewMember.created_at = DateTime.Now;
            NewMember.is_active = 1;
            NewMember.ref_member_role_id = AvailableRoles.IndexOf(NewMember.role);
            NewMember.ref_member_role_id++;
            await memberRepo.Create(NewMember);
            await LoadMembers();
            await ClearMemberLines("members");
        }
    }
}
