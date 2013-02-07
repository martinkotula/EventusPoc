using EventusPoc.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventusPoc.WebApp.ViewModels
{
    public class EventsListViewModel
    {
        public ICollection<Event> UpcommingEvents { get; set; }
        public ICollection<Event> UnaccountedEvents { get; set; }
    }
}