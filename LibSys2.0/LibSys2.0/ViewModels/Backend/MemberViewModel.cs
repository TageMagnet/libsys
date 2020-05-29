using Library;
using LibrarySystem.Etc;
using LibrarySystem.Models;
using LibrarySystem.ViewModels;
using LibrarySystem.ViewModels.Backend;
using LibrarySystem.Views.Backend;
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
        public RelayCommandWithParameters ChangeCardStatusCommand { get; set; }
        public RelayCommandWithParameters ActivateMemberCommand { get; set; }
        public RelayCommandWithParameters BookReportCommand { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<string> AvailableRoles { get; set; } = new ObservableCollection<string>() { "admin", "librarian", "user", "guest" };
        public int SelectedRoleIndex { get; set; }
        public MemberRepository memberRepo = new MemberRepository();
        public Member SelectedMember { get; set; } = new Member();
        public ObservableCollection<Member> Members { get; set; } = new ObservableCollection<Member>();
        public Member NewMember { get; set; }

        
        public string visible { get; set; } = "hidden";


        public int ActiveFilter { get; set; } = 1;
        private bool activeMemberFilter = false;
        public bool ActiveMemberFilter
        {
            get { return activeMemberFilter; }
            set
            {
                activeMemberFilter = value;
                if (value == true)
                {
                    ActiveFilter = 0;
                    visible = "visible";
                }
                else
                {
                    ActiveFilter = 1;
                    visible = "hidden";
                }
                LoadMembers();
            }
        }
        #endregion
        public MemberViewModel()
        {
            NewMember = new Member();
            AddNewMember = new RelayCommand(async () => await AddNewMemberCommand());
            DeleteMemberCommand = new RelayCommandWithParameters(async (param) => await DeleteMemberCommandMethod(param));
            UpdateMemberCommand = new RelayCommandWithParameters(async (param) => await UpdateMemberCommandMethod((Member)param));
            ChangeCardStatusCommand = new RelayCommandWithParameters(async (param) => await ChangeCardStatusCommandMethod((Member)param));
            BookReportCommand = new RelayCommandWithParameters(async (param) => await BookReportCommandMethod((Member)param));
            ActivateMemberCommand = new RelayCommandWithParameters(async (param) => await ActivateMember((Member)param));
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
                await LoadMembers();
                return;
            }
            if (member.ref_member_role_id < Globals.LoggedInUser.ref_member_role_id)
            {
                MessageBox.Show("Du kan inte uppdatera till en högre privilegie än vad du redan har");
                await LoadMembers();
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
            var x = await memberRepo.ReadAllItemsWithStatus(ActiveFilter);
            x = x.OrderBy(a => a.ref_member_role_id).ToList();
            foreach (Member member in x )
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

        public async Task ActivateMember(Member member)
        {
            await memberRepo.ActivateMember(member.member_id, 1);
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
        /// Changes the status on Loan card
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task ChangeCardStatusCommandMethod(Member member)
        {
            Member membercheck = (await memberRepo.SearchByColumn("member_id", member.member_id.ToString())).First();
            // Fix since MYSQL starts index at 1

            member.ref_member_role_id++;
            string message = "";
            //Check if the logged in user have permission to update
            if (Globals.LoggedInUser.ref_member_role_id > 1)
            {
                MessageBox.Show("Du har inte behörighet att uppdatera som en user");
                await LoadMembers();
                return;
            }
            if (membercheck.ref_member_role_id < Globals.LoggedInUser.ref_member_role_id)
            {
                MessageBox.Show("Du har inte behörighet att uppdatera en Admin");
                await LoadMembers();
                return;
            }
            if (member.ref_member_role_id < Globals.LoggedInUser.ref_member_role_id)
            {
                MessageBox.Show("Du kan inte uppdatera till en högre privilegie än vad du redan har");
                await LoadMembers();
                return;
            }

            //Chabges the status of loan card
            if (member.cardstatus == 1)
            {
                member.cardstatus = 0;
                message = "Lånekort spärrat";
            }
            else if (member.cardstatus == 0)
            {
                member.cardstatus = 1;
                message = "Lånekort aktiverat";
            }
            
            await memberRepo.Update(member);
            MessageBox.Show(message);
            await LoadMembers();
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

            if (Utilities.IsValidEmail(NewMember.email) == false)
            {
                MessageBox.Show("Fel format på email!");
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
            NewMember.cardstatus = 1;
            await memberRepo.Create(NewMember);
            await LoadMembers();
            await ClearMemberLines("members");
        }

        public async Task BookReportCommandMethod(Member member)
        {
            var reports = new ReportsView();
            reports.DataContext = new ReportsViewModel(member);
            reports.ShowDialog();
        }
    }
}
