using EventusPoc.Core.Model;
using EventusPoc.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventusPoc.WebApp.Controllers
{
    public class HomeController : Controller
    {
        EventusPocDbContext dbContext;

        public HomeController()
        {
            dbContext = new EventusPocDbContext("EventusPocDb");
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            dbContext.Events.ToList();
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return View("index");

        }

        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(Event @event)
        {
            var subscribedUsers = GetAllSubscribedUsers();
            subscribedUsers.ForEach(u => @event.EventParticipants.Add(new EventParticipant()
                {
                    User = u,
                    Event = @event,
                    IsPaid = false,
                    WasAnnounced = true,
                    WasPresent = false
                }));
            dbContext.Events.Add(@event);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }

        private List<User> GetAllSubscribedUsers()
        {
            return dbContext.Users.Where(u => u.IsSubscribed).ToList();
        }
    }
}
