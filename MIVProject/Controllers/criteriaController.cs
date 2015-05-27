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
    [CustomAuthorize(Roles = "administrator,referent")]
    public class criteriaController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: criteria
        public ActionResult Index()
        {
            return View(db.criteria.ToList());
        }

        // GET: criteria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            criteria criteria = db.criteria.Find(id);
            if (criteria == null)
            {
                return HttpNotFound();
            }
            return View(criteria);
        }

        // GET: criteria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: criteria/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "criteriaID,name")] criteria criteria)
        {
            if (ModelState.IsValid)
            {
                db.criteria.Add(criteria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(criteria);
        }

        // GET: criteria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            criteria criteria = db.criteria.Find(id);
            if (criteria == null)
            {
                return HttpNotFound();
            }
            return View(criteria);
        }

        // POST: criteria/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "criteriaID,name")] criteria criteria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(criteria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(criteria);
        }

        // GET: criteria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            criteria criteria = db.criteria.Find(id);
            if (criteria == null)
            {
                return HttpNotFound();
            }
            return View(criteria);
        }

        // POST: criteria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            criteria criteria = db.criteria.Find(id);
            db.criteria.Remove(criteria);
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
