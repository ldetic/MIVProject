using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MIVProject;

namespace MIVProject.Controllers
{
    public class ProjectItemController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: ProjectItem
        public ActionResult Index()
        {
            var projectItem = db.projectItem.Include(p => p.project1);
            return View(projectItem.ToList());
        }

        // GET: ProjectItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectItem projectItem = db.projectItem.Find(id);
            if (projectItem == null)
            {
                return HttpNotFound();
            }
            return View(projectItem);
        }

        // GET: ProjectItem/Create
        public ActionResult Create()
        {
            ViewBag.project = new SelectList(db.project, "id", "name");
            return View();
        }

        // POST: ProjectItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {
                db.projectItem.Add(projectItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        // GET: ProjectItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectItem projectItem = db.projectItem.Find(id);
            if (projectItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        // POST: ProjectItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        // GET: ProjectItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectItem projectItem = db.projectItem.Find(id);
            if (projectItem == null)
            {
                return HttpNotFound();
            }
            return View(projectItem);
        }

        // POST: ProjectItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            projectItem projectItem = db.projectItem.Find(id);
            db.projectItem.Remove(projectItem);
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
