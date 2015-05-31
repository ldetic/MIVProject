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
    [CustomAuthorize(Roles = "superuser")]
    public class SupplyStatusController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: SupplyStatus
        public ActionResult Index()
        {
            return View(db.supplyStatus.ToList());
        }

        // GET: SupplyStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyStatus supplyStatus = db.supplyStatus.Find(id);
            if (supplyStatus == null)
            {
                return HttpNotFound();
            }
            return View(supplyStatus);
        }

        // GET: SupplyStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplyStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "statusID,name")] supplyStatus supplyStatus)
        {
            if (ModelState.IsValid)
            {
                db.supplyStatus.Add(supplyStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplyStatus);
        }

        // GET: SupplyStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyStatus supplyStatus = db.supplyStatus.Find(id);
            if (supplyStatus == null)
            {
                return HttpNotFound();
            }
            return View(supplyStatus);
        }

        // POST: SupplyStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "statusID,name")] supplyStatus supplyStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplyStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplyStatus);
        }

        // GET: SupplyStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyStatus supplyStatus = db.supplyStatus.Find(id);
            if (supplyStatus == null)
            {
                return HttpNotFound();
            }
            return View(supplyStatus);
        }

        // POST: SupplyStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplyStatus supplyStatus = db.supplyStatus.Find(id);
            db.supplyStatus.Remove(supplyStatus);
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
