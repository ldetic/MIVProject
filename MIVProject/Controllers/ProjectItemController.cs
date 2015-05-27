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
    public class projectItemController : Controller
    {
        private mivEntities db = new mivEntities();

        [CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Index()
        {
            var projectItem = db.projectItem.Include(p => p.item1).Include(p => p.project1);
            return View(projectItem.ToList());
        }

        [CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
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

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Create()
        {
            ViewBag.item = new SelectList(db.item, "itemID", "name");
            ViewBag.project = new SelectList(db.project, "id", "name");
            return View();
        }

        // POST: projectItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate,item")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {
                db.projectItem.Add(projectItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.item = new SelectList(db.item, "itemID", "name", projectItem.item);
            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        // POST: projectItem/CreateViaAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public String CreateViaAjax([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate,item")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {
                db.projectItem.Add(projectItem);
                db.SaveChanges();
                return "OK";
            }
            return "ERROR";
        }

        [CustomAuthorize(Roles = "administrator,referent")]
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
            ViewBag.item = new SelectList(db.item, "itemID", "name", projectItem.item);
            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        // POST: projectItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate,item")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.item = new SelectList(db.item, "itemID", "name", projectItem.item);
            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
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

        // POST: projectItem/Delete/5
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
