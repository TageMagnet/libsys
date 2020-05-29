using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using LibrarySystem.Views.Backend;
using LibrarySystem.ViewModels.Components;

namespace LibrarySystem.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        // Aktiv användare
        public Member LoggedInCustomer { get; set; }
        // Databasrepo
        private ItemRepository itemRepo = new ItemRepository();
        /// <summary>
        /// Items that this specific user has loaned
        /// </summary>
        public ObservableCollection<OverViewItem> BorrowedItems { get; set; } = new ObservableCollection<OverViewItem>();
        public static ChangePasswordView changepw { get; set; }

        // Lämna tillbaka lånad bok
        public RelayCommandWithParameters UnsubscribeToItem { get; set; }
        // Förläng lånetid
        public RelayCommandWithParameters ExtendSubscription { get; set; }
        public RelayCommandWithParameters ChangePasswordCommand { get; set; }
        

        public CustomerViewModel()
        {
            // Sätter den inloggade usern
            LoggedInCustomer = Globals.LoggedInUser;
            LoadAllBooks();

            // Deklarera kommando för knapp
            UnsubscribeToItem = new RelayCommandWithParameters(async (param) =>
            {
                OverViewItem overViewItem = (OverViewItem)param;

                // Kod för att låna bok
                await itemRepo.UnSubscribeToItem(overViewItem, LoggedInCustomer);

                // Ta bort för visuell effekt
                BorrowedItems.Remove(overViewItem);

                MessageBox.Show("Bok återlämnad");
            });

            // Deklarera kommando för knapp2
            ExtendSubscription = new RelayCommandWithParameters(async (param) =>
            {
                // Kod för att förlänga lånetid
                OverViewItem overViewItem = (OverViewItem)param;

                // Blockera förlängning om redan försenad
                if (DateTime.Compare(overViewItem.return_at, DateTime.Now) < 0)
                {
                    MessageBox.Show("Kan ej förlänga försenad bok");
                    return;
                }

                // Förläng med X dagar
                overViewItem.return_at = overViewItem.return_at.Add(Globals.DefaultExtendLoanDuration);

                // Återanvänd lånmetoden
                await itemRepo.ResubscribeToItem(overViewItem, LoggedInCustomer);

                // Refresha view
                LoadAllBooks();

                // Display baserat på hur många default dagar ett lån är
                MessageBox.Show(string.Format("Lån förlängt med {0} dagar", Globals.DefaultExtendLoanDuration));
            });

            ChangePasswordCommand = new RelayCommandWithParameters(async (param) => { Member member = (Member)param; await ChangePasswordMethod(member); });

        }

        /// <summary>
        /// Loads all the data from DB
        /// </summary>
        public async void LoadAllBooks()
        {
            // Firstly laod books filtered by member_id and if subscribed
            await LoadBooks(LoggedInCustomer.member_id);
        }

        public async Task LoadBooks(int limiter)
        {
            BorrowedItems.Clear();
            // Only works in swedish time zone
            var now = DateTime.Now;

            foreach (var item in await itemRepo.ReadSubscribedItems(LoggedInCustomer.member_id))
            {
                // Then filter out books that are late, such a criminal
                int x = DateTime.Compare(item.return_at, now);

                // Beräkna antal dagar kvar på lån
                double DaysRemaining = (item.return_at - item.loaned_at).TotalDays;

                // Bok é försenad
                if (x < 0)
                {
                    // En converter förvandlar om färgen
                    item.LateStatus = "Försenad";
                    // Vänd till negativ
                    DaysRemaining = DaysRemaining * -1;
                }
                // Den här komemr aldrig att ske, om man inte är på millisekunden rätt
                if (x == 0)
                {
                    item.LateStatus = "Kanske";
                }
                // Allt é okay
                if (x > 0)
                {
                    item.LateStatus = "OK";
                }

                // Konvertera 'double'-datatype till en integer
                item.SubscriptionDaysRemaining = Convert.ToInt32(DaysRemaining);

                BorrowedItems.Add(item);
            }
        }

        public async Task ChangePasswordMethod(Member member)
        {
            changepw = new ChangePasswordView();
            changepw.DataContext = new ChangePasswordViewModel(member);
            changepw.ShowDialog();
        }
    }
}
