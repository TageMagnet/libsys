using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;
using LibrarySystem.Models;
using Microsoft.Win32;
using LibrarySystem.ViewModels;
using LibrarySystem.ViewModels.Backend;

namespace UnitTesting
{
    [TestClass]
    public class BasicCrudOperationsTest
    {
        [TestMethod]
        public void SQL_ExpectReturn()
        {
            //Testa connection?
            //var repo = new MemberRepository();  
        }

        [TestMethod]
        public void ReportsTests()
        {
            Member member = new Member();
            member.member_id = 47;

            ReportsViewModel reportsViewModel = new ReportsViewModel(member);
        }
    }
}
