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
    public class SupplierCategoryController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: SupplierCategory
        public ActionResult Index()
        {
            return View(db.supplierCategory.ToList());
        }

        // GET: SupplierCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplierCategory supplierCategory = db.supplierCategory.Find(id);
            if (supplierCategory == null)
            {
                return HttpNotFound();
            }
            return View(supplierCategory);
        }

        // GET: SupplierCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] supplierCategory supplierCategory)
        {
            if (ModelState.IsValid)
            {
                db.supplierCategory.Add(supplierCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplierCategory);
        }

        // GET: SupplierCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplierCategory supplierCategory = db.supplierCategory.Find(id);
            if (supplierCategory == null)
            {
                return HttpNotFound();
            }
            return View(supplierCategory);
        }

        // POST: SupplierCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] supplierCategory supplierCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplierCategory);
        }

        // GET: SupplierCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplierCategory supplierCategory = db.supplierCategory.Find(id);
            if (supplierCategory == null)
            {
                return HttpNotFound();
            }
            return View(supplierCategory);
        }

        // POST: SupplierCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplierCategory supplierCategory = db.supplierCategory.Find(id);
            db.supplierCategory.Remove(supplierCategory);
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
