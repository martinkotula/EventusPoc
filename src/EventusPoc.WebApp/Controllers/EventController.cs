using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventusPoc.Core.Model;
using EventusPoc.DataAccess;
using EventusPoc.WebApp.ViewModels;
using EventusPoc.WebApp.Services;

namespace EventusPoc.WebApp.Controllers
{
    public class EventController : Controller
    {
        private readonly EventusPocDbContext db;
        private readonly EventService eventService;

        public EventController()
        {
            db = new EventusPocDbContext("EventusPocDb");
            this.eventService = new EventService(db);
        }

        //
        // GET: /Event/

        public ActionResult Index()
        {
            EventsListViewModel vm = new EventsListViewModel()
            {
                UpcommingEvents = eventService.GetUpcommingEvents(),
                ArchivedEvents = eventService.GetArchivedEvents()
            };

            return View(vm);
        }

        //
        // GET: /Event/Details/5

        public ActionResult Details(int id = 0)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        //
        // GET: /Event/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Event/Create

        [HttpPost]
        public ActionResult Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Users.Where(u => u.IsSubscribed).ToList().ForEach((u) =>
                    {
                        db.EventParticipants.Add(new EventParticipant()
                            {
                                Event = @event,
                                User = u,
                            });
                    }
                );
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        public ActionResult Edit(int id = 0)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        [HttpPost]
        public ActionResult Edit(Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        public ActionResult Delete(int id = 0)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SettleDebts(int id)
        {
            Event @event = db.Events.Find(id);
            return View(@event);
        }

        [ChildActionOnly]
        public PartialViewResult ParticipantList(int eventId)
        {
            var @event = db.Events.Find(eventId);
            ParticipantListViewModel vm = new ParticipantListViewModel()
            {
                EventId = eventId,
                EventParticipants = @event.EventParticipants.ToList(),
                AvailableUsers = GetAvailableUserForEvent(@event)
            };

            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult ParticipantList(ParticipantListViewModel vm)
        {
            var @event = db.Events.Find(vm.EventId);
            var user = db.Users.Find(vm.AddedUserId);
            AddParticipantToEvent(@event, user);
            
            vm.EventParticipants = @event.EventParticipants.ToList();
            vm.AvailableUsers = GetAvailableUserForEvent(@event);

            return PartialView(vm);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private IEnumerable<SelectListItem> GetAvailableUserForEvent(Event @event)
        {
            var participants = @event.EventParticipants.Select(ep => ep.User.Id).ToList();


            var availableUsers = from u in db.Users
                                 where !participants.Contains(u.Id)
                                 select u;

            return availableUsers.ToList().Select(au => new SelectListItem()
                {
                    Text = au.Login,
                    Value = au.Id.ToString()
                }).ToList(); 
            //var availableUsers = db.Users.Except(participants).ToList();
            //return availableUsers.Select(p => 
        }


        private void AddParticipantToEvent(Event @event, User user)
        {
            if (@event != null && user != null)
            {
                @event.EventParticipants.Add(new EventParticipant()
                {
                    Event = @event,
                    IsPaid = false,
                    User = user,
                    WasAnnounced = true,
                    WasPresent = true
                });

                db.SaveChanges();
            }
        }

    }
}