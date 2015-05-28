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

    public class ItemController : Controller
    {
        private mivEntities db = new mivEntities();

        //[CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Index()
        {
            var item = db.item.Include(i => i.itemSubCategory);


            return View(item.ToList());
        }

        //[CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Create()
        {
            ViewBag.subcategory = new SelectList(db.itemSubCategory, "subCategoryID", "name");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "itemID,name,subcategory,description,unitOfMeasure,quantity")] item item)
        {
            if (ModelState.IsValid)
            {
                db.item.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.subcategory = new SelectList(db.itemSubCategory, "subCategoryID", "name", item.subcategory);
            return View(item);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.subcategory = new SelectList(db.itemSubCategory, "subCategoryID", "name", item.subcategory);
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "itemID,name,subcategory,description,unitOfMeasure,quantity")] item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.subcategory = new SelectList(db.itemSubCategory, "subCategoryID", "name", item.subcategory);
            return View(item);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            item item = db.item.Find(id);
            db.item.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Item/DeleteViaAjax/5
        [HttpPost, ActionName("DeleteViaAjax")]
        [ValidateAntiForgeryToken]
        public String DeleteConfirmedAjax(int id)
        {
            try
            {
                item item = db.item.Find(id);
                db.item.Remove(item);
                db.SaveChanges();
                return "OK";
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
