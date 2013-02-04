using EventusPoc.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventusPoc.WebApp.ViewModels
{
    public class ParticipantListViewModel
    {
        public int EventId { get; set; }
        public List<EventParticipant> EventParticipants { get; set; }
        public int AddedUserId { get; set; }

        public IEnumerable<SelectListItem> AvailableUsers { get; set; }
    }
}