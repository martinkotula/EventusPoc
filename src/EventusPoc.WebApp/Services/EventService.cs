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
            return dbContext.Events.Where(e => e.Date >= DateTime.Now).OrderBy(e => e.Date).ToList();
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
    }
}