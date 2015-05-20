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
    public class SupplierController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: Supplier
        public ActionResult Index()
        {
            var supplier = db.supplier.Include(s => s.mivUser1).Include(s => s.supplierCategory);
            return View(supplier.ToList());
        }

        // GET: Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = db.supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            ViewBag.mivUser = new SelectList(db.mivUser, "userID", "username");
            ViewBag.category = new SelectList(db.supplierCategory, "id", "name");
            return View();
        }

        // POST: Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mivUser,name,phone,email,category,OIB")] supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.supplier.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.mivUser = new SelectList(db.mivUser, "userID", "username", supplier.mivUser);
            ViewBag.category = new SelectList(db.supplierCategory, "id", "name", supplier.category);
            return View(supplier);
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = db.supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.mivUser = new SelectList(db.mivUser, "userID", "username", supplier.mivUser);
            ViewBag.category = new SelectList(db.supplierCategory, "id", "name", supplier.category);
            return View(supplier);
        }

        // POST: Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mivUser,name,phone,email,category,OIB")] supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.mivUser = new SelectList(db.mivUser, "userID", "username", supplier.mivUser);
            ViewBag.category = new SelectList(db.supplierCategory, "id", "name", supplier.category);
            return View(supplier);
        }

        // GET: Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = db.supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplier supplier = db.supplier.Find(id);
            db.supplier.Remove(supplier);
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
