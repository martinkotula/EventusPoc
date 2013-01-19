using EventusPoc.Core.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventusPoc.DataAccess
{
    public class EventusPocDbContext : DbContext
    {
        public EventusPocDbContext()
            : base()
        {

        }
        public EventusPocDbContext(string connectionString) : base(connectionString) { }
 
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
    }
}
