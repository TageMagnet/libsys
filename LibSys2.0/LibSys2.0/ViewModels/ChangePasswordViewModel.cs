using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibrarySystem.ViewModels.Components
{
    class ChangePasswordViewModel : BaseViewModel
    {
        public MemberRepository MemberRepo = new MemberRepository();
        public Member currentMember { get; set; }
        public RelayCommandWithParameters ChangePasswordCommand {get;set;}
        public string CurrentPw { get; set; }
        public string NewPw { get; set; }
        public string NewPwCheck { get; set; }
        public ChangePasswordViewModel()
        {

        }
        public ChangePasswordViewModel(Member member)
        {
            currentMember = member;
            ChangePasswordCommand = new RelayCommandWithParameters(async (param) => { Member member = (Member)param; await UpdatePw(member); });  
        }

        public async Task UpdatePw(Member member)
        {
            if (CurrentPw != member.pwd)
            {
                MessageBox.Show("Fel lösenord!");
                await ClearPassword();
                return;
            }
            if (String.IsNullOrWhiteSpace(NewPw) || String.IsNullOrEmpty(NewPw))
            {
                MessageBox.Show("Fyll i Lösenord!");
                await ClearPassword();
                return;
            }
            if (NewPw.Length < 6)
            {
                MessageBox.Show("Lösenordet måste bestå av minst 6 tecken!");
                await ClearPassword();
                return;
            }
            if (NewPw != NewPwCheck)
            {
                MessageBox.Show("Lösenorden stämde inte överens.");
                await ClearPassword();
                return;
            }
            member.pwd = NewPw;
            await MemberRepo.UpdatePassword(member);
            MessageBox.Show("Lösenordet uppdaterat.");
            CustomerViewModel.changepw.Close();
        }

        public async Task ClearPassword()
        {
            CurrentPw = "";
            NewPw = "";
            NewPwCheck= "";
        }
    }
}
