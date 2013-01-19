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
    public class PlayerController : Controller
    {
        private EventusPocDbContext db = new EventusPocDbContext("EventusPocDb");

        //
        // GET: /Player/

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //
        // GET: /Player/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /Player/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = null;
            if (id == 0)
                user = new User();
            else
                user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        //
        // POST: /Player/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = user.Id == 0 ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(user);
        }

        //
        // GET: /Player/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Player/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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