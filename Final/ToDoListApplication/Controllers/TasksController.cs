using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoListApplication.Models;

namespace ToDoListApplication.Controllers
{
    public class TasksController : Controller
    {
        private ToDoListDBContext db = new ToDoListDBContext();


        // GET: Tasks/ExpiredTasks
        public ActionResult ExpiredTasks()
        {
            var tasks = db.Tasks.Where(a => a.DueDate < DateTime.Today).ToList();
            return Json(tasks,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteExpiredTasks()
        {
            var tasks = db.Tasks.Where(a => a.DueDate < DateTime.Today).ToList();
            foreach(var task in tasks)
            {
                db.Tasks.Remove(task);
            }
            db.SaveChanges();

            return Json(new { count = tasks.Count(), status="Success"});
        }

        // GET: Tasks
        public ActionResult UrgentTasks(int nbr)
        {                      
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays((double)nbr);

            var tasks = db.Tasks.Where(a => (DateTime.Compare(a.DueDate,startDate) >= 0) 
                                         && (DateTime.Compare(a.DueDate,endDate) <= 0)).ToList();
            return Json(tasks,JsonRequestBehavior.AllowGet);           
        }

        // GET: Tasks
        public ActionResult SearchByCategory(string searchstring)
        {
            var tasks = db.Tasks.Where(a => a.Category.Title == searchstring).ToList();
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        // GET: Tasks
        public ActionResult Index()
        {
            return View(db.Tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Title");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,Title,DueDate")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(tasks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Title");
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,Title,DueDate")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tasks tasks = db.Tasks.Find(id);
            db.Tasks.Remove(tasks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
