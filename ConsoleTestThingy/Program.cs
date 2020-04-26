using System;
using System.Collections.Generic;
using Library;
using LIBSYS.Models;
using System.Threading.Tasks;

namespace ConsoleTestThingy
{
    class Program
    {
        public static void Main(string[] args)
        {
            RunStuff().Wait();
            RunOtherStuff().Wait();
            WaitToQuit().Wait();

        }
        public static async Task RunStuff()
        {
            // Declare repo based on model
            // if none exist, feel free to create new models/repositories
            var memberRepo = new Library.MemberRepository();
            var eventRepo = new Library.EventRepository();
            var bookRepo = new Library.BookRepository();

            var member = new Member
            {
                created_at = DateTime.Now,
                email = "dfghjklö",
                nickname = "qwertyöä",
                pwd = "secret",
                role = "user"
            }; //, roles = new List<string>() { "admin", "clown", "visitor" }

            var _event = new Event
            {
                event_type = "bokklubb",
                time_start = DateTime.Now,
                time_end = DateTime.Now,
                location = "Rum A",
                owner = 3
            };
            var book = new Book
            {
                title = "The Sapphire Rose",
                isbn = "9780586203743",
                year = 1998,
                description = "Has a sword dude on the cover page",
                condition = 100

            };
            Console.WriteLine(bookRepo.GenerateUpdateQuery(book));

            //var book = new Book 
            //{
            //    title = "The Sapphire Rose",
            //    0586203745 / 
            //    9780586203743          
            //}
            //Update
            //Create
            //await event_repo.Create(_event);
            //ReadAll
            //foreach(var item in await repo.ReadAll())
            //{
            //    Console.WriteLine(item.email +""+item.role);
            //}
            //Delete
        }
        static private async Task<int> RunOtherStuff()
        {
            bool loopIsRunning = true;
            int iterations = 0;

            Console.WriteLine("RunOtherStuff() is running");

            while (loopIsRunning)
            {
                await Task.Delay(200); // 0,2 sec delay
                Console.Write('.');

                if (iterations >= 6)
                    loopIsRunning = false;

                iterations++;
            }
 
            Console.WriteLine("\nRunOtherStuff() Finished");
            return 1;
        }

        static private async Task WaitToQuit()
        {
            Console.Write("Press any key to quit..\n");
            Console.ReadKey();
        }

    }
}
