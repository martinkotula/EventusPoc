using EventusPoc.Core.Model;
using EventusPoc.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            return dbContext.Events.Where(e => e.Date >= DateTime.Now).ToList();
        }

        public ICollection<Event> GetArchivedEvents(int? n)
        {
            // TODO datetime.now from dateTime provider
            var events = dbContext.Events.Where(e => e.Date < DateTime.Now);
            if (n.HasValue)
                events = events.Take(n.Value);
            return events.ToList();
        }
    }
}