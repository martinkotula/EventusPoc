using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventusPoc.Core.Model;
using EventusPoc.DataAccess;

namespace EventusPoc.WebApp.Controllers
{
    public class EventController : Controller
    {
        private EventusPocDbContext db = new EventusPocDbContext("EventusPocDb");

        //
        // GET: /Event/

        public ActionResult Index()
        {
            return View(db.Events.ToList());
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
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        //
        // GET: /Event/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        //
        // POST: /Event/Edit/5

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

        //
        // GET: /Event/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        //
        // POST: /Event/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}