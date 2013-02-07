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
                UnaccountedEvents = eventService.GetUnaccountedEvents()
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
            SettleDebtsViewModel vm = new SettleDebtsViewModel()
            {
                EventId = id,
                EventDate = @event.FormattedDate,
                AvailableUsers = eventService.GetAvailableUsersForEvent(@event.EventId),
                Participants = @event.EventParticipants.Select(p => new SettleDebtsPlayerViewModel()
                {
                    Login = p.User.Login,
                    UserId = p.User.Id,
                    WasPresent = p.WasPresent,
                    HasPaid = p.IsPaid
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult SettleDebts(SettleDebtsViewModel vm)
        {
            if (vm.IsAddPlayerButtonClicked)
            {
                User user = db.Users.Find(vm.AddedUserId);
                vm.Participants.Add(new SettleDebtsPlayerViewModel()
                {
                    UserId = vm.AddedUserId,
                    Login = user.Login,
                    HasPaid = false,
                    WasPresent = true
                });
                vm.AvailableUsers = eventService.GetAvailableUsersForEvent(vm.EventId);
                return View(vm);
            }
            if (vm.IsSaveButtonClicked)
            {
                Event @event = db.Events.Find(vm.EventId);
                foreach (var participant in vm.Participants)
                {
                    EventParticipant ep = db.EventParticipants.Where(p => p.User.Id == participant.UserId && p.Event.EventId == vm.EventId).SingleOrDefault();
                    if (ep == null)
                    {
                        // uzytkownik dodany przy rozliczeniu
                        ep = new EventParticipant()
                        {
                            Event = @event,
                            User = db.Users.Find(participant.UserId),
                            IsPaid = participant.HasPaid,
                            WasPresent = participant.WasPresent,
                            WasAnnounced = false
                        };
                        db.EventParticipants.Add(ep);
                        db.SaveChanges();
                    }
                    else
                    {
                        // uzytkownik zapisany wczesniej
                        ep.IsPaid = participant.HasPaid;
                        ep.WasAnnounced = true;
                        ep.WasPresent = participant.WasPresent;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("index");
            }
            
            return View(vm);
        }

        [ChildActionOnly]
        public PartialViewResult ParticipantList(int eventId)
        {
            var @event = db.Events.Find(eventId);
            ParticipantListViewModel vm = new ParticipantListViewModel()
            {
                EventId = eventId,
                EventParticipants = @event.EventParticipants.ToList(),
                AvailableUsers = eventService.GetAvailableUsersForEvent(eventId)
            };

            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult ParticipantList(ParticipantListViewModel vm)
        {
            var @event = db.Events.Find(vm.EventId);
            eventService.AddParticipantToEvent(@event.EventId, vm.AddedUserId);
            
            vm.EventParticipants = @event.EventParticipants.ToList();
            vm.AvailableUsers = eventService.GetAvailableUsersForEvent(vm.EventId);

            return PartialView(vm);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}