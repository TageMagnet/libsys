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
            var event_repo = new Library.EventRepository();
            var member = new Member { created_at = DateTime.Now, email = "dfghjklö", nickname = "qwertyöä", pwd = "secret" }; //, roles = new List<string>() { "admin", "clown", "visitor" }
            var _event = new Event { event_type = "bokklubb", time_start = DateTime.Now, time_end = DateTime.Now, location = "Rum A", owner = 3 };

            //Create
            //await event_repo.Create(_event);

            //ReadAll
            //foreach(var item in await repo.ReadAll())
            //{
            //    Console.WriteLine(item.email +""+item.role);
            //}

            //Update

            //Delete
        }
    }
}
