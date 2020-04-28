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

        public List<LIBSYS.Models.Event> ListOfEvents { get; set; }

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
