using Library;
using LibrarySystem.Etc;
using LibrarySystem.Models;
using ReactiveUI;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibrarySystem.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Properties
        public MemberRepository memberRepo = new MemberRepository();
        public Member NewMember { get; set; }
        public string CheckPassword { get; set; }

        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; set; }

        #endregion

        public RegisterViewModel()
        {
            NewMember = new Member();

            RegisterCommand = ReactiveCommand.CreateFromTask(() => RegisterCommandMethod());
        }


        public async Task RegisterCommandMethod()
        {
            if (String.IsNullOrEmpty(NewMember.email))
            {
                MessageBox.Show("Fyll i email!");
                return;
            }
            if (Utilities.IsValidEmail(NewMember.email) == false)
            {
                MessageBox.Show("Fel format på email!");
                return;
            }
            if (String.IsNullOrWhiteSpace(NewMember.nickname))
            {
                MessageBox.Show("Fyll i namn!");
                return;
            }
            if (String.IsNullOrWhiteSpace(NewMember.pwd) || String.IsNullOrEmpty(NewMember.pwd))
            {
                MessageBox.Show("Fyll i Lösenord!");
                return;
            }
            if (NewMember.pwd.Length < 6)
            {
                MessageBox.Show("Lösenordet måste bestå av minst 6 tecken!");
                return;
            }
            if (NewMember.pwd != CheckPassword)
            {
                MessageBox.Show("Lösenorden stämmer inte överens");
                return;
            }

            NewMember.ref_member_role_id = 2;
            NewMember.created_at = DateTime.Now;
            NewMember.is_active = 1;

            Mail mail = new Mail();
            mail.SendTo(NewMember.email);
            MessageBox.Show("Välkommen! Du har fått en bekräftelse till din email.");
            MainWindowViewModel.ChangeView("home");

            await memberRepo.Create(NewMember);
        }
    }
}
