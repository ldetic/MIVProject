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
        public ActionResult Index()
        {
            var project = db.project.Include(p => p.deliveryMethod1).Include(p => p.paymentMethod1);
            return View(project.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
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

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name");
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name");
            return View();
        }

        // GET: Project/CreateNew
        public ActionResult CreateNew()
        {
            var project = new MIVProject.project();
            //var item = new MIVProject.item();
            var projectItem = new MIVProject.projectItem();

            var item = db.item.Include(i => i.itemSubCategory);
            ViewBag.items = item;
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name");
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name");
            return View(Tuple.Create(project, projectItem));
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
                return RedirectToAction("Index");
            }

            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", project.deliveryMethod);
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", project.paymentMethod);
            return View(project);
        }

        // GET: Project/Edit/5
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
            try {
                if (ModelState.IsValid)
                {
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", project.deliveryMethod);
                ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", project.paymentMethod);
                return View(project);
            } catch(Exception ex)
            {
                return Edit(currentEditId);
            }
        }

        // GET: Project/Delete/5
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
            project project = db.project.Find(id);
            db.project.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Project/DeleteViaAjax/5
        [HttpPost, ActionName("DeleteViaAjax")]
        [ValidateAntiForgeryToken]
        public String DeleteConfirmedAjax(int id)
        {
            try { 
                project project = db.project.Find(id);
                db.project.Remove(project);
                db.SaveChanges();
                return "OK";
            } catch(Exception ex)
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
