using EventusPoc.Core.Model;
using EventusPoc.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventusPoc.WebApp.Services
{
    public class EventService
    {
        private readonly EventusPocDbContext dbContext;

        public EventService(EventusPocDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            this.dbContext = dbContext;
        }

        public ICollection<Event> GetUpcommingEvents()
        {
            // TODO datetime.now from dateTime provider
            return dbContext.Events.Where(e => e.Date >= DateTime.Now).OrderBy(e => e.Date).ToList();
        }


        public ICollection<Event> GetUnaccountedEvents()
        {
            return dbContext.Events.Where(e => e.Date < DateTime.Now && !e.IsAccounted).OrderByDescending(e => e.Date).ToList();
        }

        public ICollection<Event> GetArchivedEvents(int? n = null)
        {
            // TODO datetime.now from dateTime provider
            var events = dbContext.Events.Where(e => e.Date < DateTime.Now).OrderByDescending(e => e.Date);
            if (n.HasValue)
                return events.Take(n.Value).ToList();
            else 
                return events.ToList();
        }

        public IEnumerable<SelectListItem> GetAvailableUsersForEvent(int eventId)
        {
            Event @event = dbContext.Events.Find(eventId);
            var participants = @event.EventParticipants.Select(ep => ep.User.Id).ToList();


            var availableUsers = from u in dbContext.Users
                                 where !participants.Contains(u.Id)
                                 select u;

            return availableUsers.ToList().Select(au => new SelectListItem()
            {
                Text = au.Login,
                Value = au.Id.ToString()
            }).ToList();
        }

        public void AddParticipantToEvent(int eventId, int userId)
        {
            Event @event = dbContext.Events.Find(eventId);
            User user = dbContext.Users.Find(userId);
            // TODO surround with transaction scope
            if (@event != null && user != null && 
                !@event.EventParticipants.Any(p => p.User.Id == user.Id))
            {
                @event.EventParticipants.Add(new EventParticipant()
                {
                    Event = @event,
                    IsPaid = false,
                    User = user,
                    WasAnnounced = true,
                    WasPresent = true
                });

                dbContext.SaveChanges();
            }
        }

        public Event GetNextEvent()
        {
            return this.GetUnaccountedEvents().FirstOrDefault();
        }
    }
}