using K4os.Compression.LZ4.Internal;
using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.ViewModels.Backend
{
    public class ReportsViewModel: BaseViewModel
    {
        #region Properties
        public List<OverViewItem> Items { get; set; } = new List<OverViewItem>();

        public ItemRepository itemRepo = new ItemRepository();

        public Member CurrentMember { get; set; } = new Member();

        public List<OverViewItem> CurrentLoans { get; set; } = new List<OverViewItem>();

        #endregion
        // Empty for no construtor arguments
        public ReportsViewModel() {
            ReadOtherData();
        }
        public ReportsViewModel(Member member)
        {
            CurrentMember = member;
            ReadData(member);
        }

        public async void ReadData(Member member)
        {
            await GetData(member);
        }

        public async void ReadOtherData()
        {
            await GetOtherData();
        }

        public async Task GetData(Member member)
        {
            var now = DateTime.Now;

            foreach (var item in await itemRepo.ReadSubscribedItems(CurrentMember.member_id))
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

                CurrentLoans.Add(item);
            }
        }

        public async Task GetOtherData()
        {
            foreach (var item in await itemRepo.ReadAllItemsWithStatus2(1, 25))
            {
                // todo; incorrect. Used as a placeholder for now
                item.loaned_at = Etc.Utilities.RandomDate();
                Items.Add(item);
            }
        }
    }
}
