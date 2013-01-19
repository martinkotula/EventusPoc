using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventusPoc.Core.Model
{
    public class EventParticipant
    {
        public int EventParticipantId { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public bool IsPaid { get; set; }
        public bool WasPresent { get; set; }
        public bool WasAnnounced { get; set; }
    }
}
