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
    public class SupplyHeaderController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: SupplyHeader
        public ActionResult Index()
        {
            var supplyHeader = db.supplyHeader.Include(s => s.currency1).Include(s => s.deliveryMethod1).Include(s => s.paymentMethod1).Include(s => s.project1).Include(s => s.supplier1).Include(s => s.supplyStatus);
            return View(supplyHeader.ToList());
        }

        // GET: SupplyHeader/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyHeader supplyHeader = db.supplyHeader.Find(id);
            if (supplyHeader == null)
            {
                return HttpNotFound();
            }
            return View(supplyHeader);
        }

        // GET: SupplyHeader/Create
        public ActionResult Create()
        {
            ViewBag.currency = new SelectList(db.currency, "currencyID", "name");
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name");
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name");
            ViewBag.project = new SelectList(db.project, "id", "name");
            ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name");
            ViewBag.status = new SelectList(db.supplyStatus, "statusID", "name");
            return View();
        }

        // POST: SupplyHeader/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "supplyID,paymentMethod,deliveryMethod,paymentDate,deliveryDate,supplier,date,project,status,currency")] supplyHeader supplyHeader)
        {
            if (ModelState.IsValid)
            {
                db.supplyHeader.Add(supplyHeader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.currency = new SelectList(db.currency, "currencyID", "name", supplyHeader.currency);
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", supplyHeader.deliveryMethod);
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", supplyHeader.paymentMethod);
            ViewBag.project = new SelectList(db.project, "id", "name", supplyHeader.project);
            ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name", supplyHeader.supplier);
            ViewBag.status = new SelectList(db.supplyStatus, "statusID", "name", supplyHeader.status);
            return View(supplyHeader);
        }

        // GET: SupplyHeader/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyHeader supplyHeader = db.supplyHeader.Find(id);
            if (supplyHeader == null)
            {
                return HttpNotFound();
            }
            ViewBag.currency = new SelectList(db.currency, "currencyID", "name", supplyHeader.currency);
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", supplyHeader.deliveryMethod);
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", supplyHeader.paymentMethod);
            ViewBag.project = new SelectList(db.project, "id", "name", supplyHeader.project);
            ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name", supplyHeader.supplier);
            ViewBag.status = new SelectList(db.supplyStatus, "statusID", "name", supplyHeader.status);
            return View(supplyHeader);
        }

        // POST: SupplyHeader/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "supplyID,paymentMethod,deliveryMethod,paymentDate,deliveryDate,supplier,date,project,status,currency")] supplyHeader supplyHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplyHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.currency = new SelectList(db.currency, "currencyID", "name", supplyHeader.currency);
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", supplyHeader.deliveryMethod);
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", supplyHeader.paymentMethod);
            ViewBag.project = new SelectList(db.project, "id", "name", supplyHeader.project);
            ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name", supplyHeader.supplier);
            ViewBag.status = new SelectList(db.supplyStatus, "statusID", "name", supplyHeader.status);
            return View(supplyHeader);
        }

        // GET: SupplyHeader/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyHeader supplyHeader = db.supplyHeader.Find(id);
            if (supplyHeader == null)
            {
                return HttpNotFound();
            }
            return View(supplyHeader);
        }

        // POST: SupplyHeader/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplyHeader supplyHeader = db.supplyHeader.Find(id);
            db.supplyHeader.Remove(supplyHeader);
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
