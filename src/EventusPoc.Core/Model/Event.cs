using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventusPoc.Core.Model
{
    public class Event
    {
        public Event()
        {
            this.EventParticipants = new List<EventParticipant>();
        }
        public int EventId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public bool IsAccounted { get; set; }

        public virtual ICollection<EventParticipant> EventParticipants { get; set; }

        public string FormattedDate
        {
            get
            {
                return this.Date.ToString("dddd, d MMMM yyyy, hh:mm");
            }
        }

        public string FormattedShortDate
        {
            get
            {
                return this.Date.ToString("dd.MM, dddd");
            }
        }


    }
}
