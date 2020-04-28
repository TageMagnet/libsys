using LIBSYS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIBSYS.ViewModels
{
    class LibrarianViewModel
    {
        //public Repository repo = new Repository();
        //public string Title { get; set; }
        //public double Completion { get; set; }

        public List<Event> ListOfEvents { get; set; }
        public List<Book> Books;
        public LibrarianViewModel()
        {
            Books = new List<Book>();
            Books.Add(new Book { title = "Harry Potter", description = "HARRY BECOMES A WIZZARD YO!" });
            Books.Add(new Book { title = "Harry Potter 2", description = "HARRY IS NOW A WIZZARD" });
            Books.Add(new Book { title = "Harry Potter 3", description = "HARRY LOST HIS WAND" });
        }
        //private ObservableCollection<Event> events;
        //public ObservableCollection<Event> Events
        //{
        //    get { return events; }
        //    set
        //    {
        //        var eventIE = repo.ReadEvents();
        //        events = new ObservableCollection<Event>(eventIE.ToList());
        //    }
        //}
    }
}
