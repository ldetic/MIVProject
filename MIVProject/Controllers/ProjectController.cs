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
    public class ProjectController : Controller
    {
        private mivEntities db = new mivEntities();
        private int currentEditId;
        // GET: Project
        //[CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Index()
        {
            var project = db.project.Include(p => p.deliveryMethod1).Include(p => p.paymentMethod1);
            return View(project.ToList());
        }

        //[CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.project.Find(id);
            IQueryable<projectItem> projectItems = db.projectItem.Where(c => c.project == id);

            ViewBag.projectItems = projectItems;
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Create()
        {
            //included because we need to select items during process of creating new project
            var item = db.item.Include(i => i.itemSubCategory);
            ViewBag.items = item;

            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name");
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,paymentMethod,paymentDate,deliveryMethod,deliveryDate,description")] project project)
        {
            if (ModelState.IsValid)
            {
                db.project.Add(project);
                db.SaveChanges();

                string username = Session["username"].ToString();
                DateTime date = DateTime.Now;
                string msg = "Project created " + project.name + " id:" + project.id;
                db.Database.ExecuteSqlCommand("Insert into logs values(0, @p0, @p1, @p2 )", username, msg, date);

                return RedirectToAction("Index");
            }

            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", project.deliveryMethod);
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", project.paymentMethod);
            return View(project);
        }

        // POST: Project/CreateViaAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public String CreateViaAjax([Bind(Include = "id,name,paymentMethod,paymentDate,deliveryMethod,deliveryDate,description")] project project)
        {
            if (ModelState.IsValid)
            {
                db.project.Add(project);
                db.SaveChanges();

                string username = Session["username"].ToString();
                DateTime date = DateTime.Now;
                string msg = "Project created " + project.name + " id:" + project.id;
                db.Database.ExecuteSqlCommand("Insert into logs values(0, @p0, @p1, @p2 )", username, msg, date);

                return project.id.ToString();
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
            project project = db.project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", project.deliveryMethod);
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", project.paymentMethod);
            currentEditId = (int)id;
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,paymentMethod,paymentDate,deliveryMethod,deliveryDate,description")] project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();

                    string username = Session["username"].ToString();
                    DateTime date = DateTime.Now;
                    string msg = "Project edited " + project.name + " id:" + project.id;
                    db.Database.ExecuteSqlCommand("Insert into logs values(0, @p0, @p1, @p2 )", username, msg, date);

                    return RedirectToAction("Index");
                }
                ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", project.deliveryMethod);
                ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", project.paymentMethod);
                return View(project);
            }
            catch
            {
                return Edit(currentEditId);
            }
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*
            mivEntities mivEntities = new mivEntities();
            var supply = mivEntities.supplyHeader.Where(x => x.project == id).Count();
            if (supply == 0)
            {
                db.Database.ExecuteSqlCommand("Delete from projectItem where project = @p0", id);
            }
            */

            project project = db.project.Find(id);
            if (project != null)
            {
                db.Database.ExecuteSqlCommand("Delete from projectItem where project = @p0", id);
                db.project.Remove(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: Project/DeleteViaAjax/5
        [HttpPost, ActionName("DeleteViaAjax")]
        [ValidateAntiForgeryToken]
        public String DeleteConfirmedAjax(int id)
        {
            try
            {
                project project = db.project.Find(id);
                if (project != null)
                {
                    db.Database.ExecuteSqlCommand("Delete from projectItem where project = @p0", id);
                    db.project.Remove(project);
                    db.SaveChanges();

                    string username = Session["username"].ToString();
                    DateTime date = DateTime.Now;
                    string msg = "Project edited " + project.name + " id:" + project.id;
                    db.Database.ExecuteSqlCommand("Insert into logs values(0, @p0, @p1, @p2 )", username, msg, date);

                    return "OK";
                }
                return "ERROR";
            }
            catch
            {
                return "ERROR";
            }
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
