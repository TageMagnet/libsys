using K4os.Compression.LZ4.Internal;
using Library;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    /// <summary>
    /// Doing it hard and statically like it's no one's business
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Default book time set to 8 days
        /// <para>Timepspan has these params => (days, hours, minutes, seconds ..)</para>
        /// </summary>
        public static TimeSpan DefaultLoanDuration { get; set; } = new TimeSpan(8, 0, 0, 0);

        public static  MemberRepository memerRepo = new MemberRepository();
        /// <summary>
        /// Shakey practice but eh.
        /// </summary>
        public static Member LoggedInUser {get;set;} = new Member();

        /// <summary>
        /// Simple bool check if valid user is logged in
        /// </summary>
        public static bool IsLoggedIn
        {
            get
            {
                if(LoggedInUser.ref_member_role_id < 1 || (string.IsNullOrEmpty(LoggedInUser.email) && string.IsNullOrWhiteSpace(LoggedInUser.email)))
                    return false;
                return true;
            }
        }

        public static async Task UpdateGlobalUser()
        {
            Member user = (await memerRepo.SearchByColumn("member_id", LoggedInUser.member_id.ToString())).First();

            LoggedInUser = user;

        }
    }
}


