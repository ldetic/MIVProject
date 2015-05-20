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
    public class DeliveryMethodController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: DeliveryMethod
        public ActionResult Index()
        {
            return View(db.deliveryMethod.ToList());
        }

        // GET: DeliveryMethod/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deliveryMethod deliveryMethod = db.deliveryMethod.Find(id);
            if (deliveryMethod == null)
            {
                return HttpNotFound();
            }
            return View(deliveryMethod);
        }

        // GET: DeliveryMethod/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryMethod/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "deliveryID,name")] deliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                db.deliveryMethod.Add(deliveryMethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryMethod);
        }

        // GET: DeliveryMethod/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deliveryMethod deliveryMethod = db.deliveryMethod.Find(id);
            if (deliveryMethod == null)
            {
                return HttpNotFound();
            }
            return View(deliveryMethod);
        }

        // POST: DeliveryMethod/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "deliveryID,name")] deliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryMethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deliveryMethod);
        }

        // GET: DeliveryMethod/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deliveryMethod deliveryMethod = db.deliveryMethod.Find(id);
            if (deliveryMethod == null)
            {
                return HttpNotFound();
            }
            return View(deliveryMethod);
        }

        // POST: DeliveryMethod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            deliveryMethod deliveryMethod = db.deliveryMethod.Find(id);
            db.deliveryMethod.Remove(deliveryMethod);
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
