using System;
using System.Collections.Generic;
using Library;
using LIBSYS.Models;
using System.Threading.Tasks;

namespace ConsoleTestThingy
{
    class Program
    {
        static async Task Main(string[] args)
        { 
            var result = RunStuff();

            Console.Write("Press any key to quit..\n");
            Console.ReadKey();
        }
        static async Task RunStuff()
        {
            var repo = new Library.MemberRepository();
            var member = new Member { email = "dfghjklö", nickname = "qwertyöä", pwd = "secret", roles = new List<string>() { "admin", "clown", "visitor" } };
        
            await repo.Create(member);
            var xy = 0;
            var x = 0;
            Console.WriteLine(member);
            //var res = await repo.Read(4);
        }
    }
}
