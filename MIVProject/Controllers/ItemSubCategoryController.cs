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
    public class ItemSubCategoryController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: ItemSubCategory
        public ActionResult Index()
        {
            var itemSubCategory = db.itemSubCategory.Include(i => i.itemCategory);
            return View(itemSubCategory.ToList());
        }

        // GET: ItemSubCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itemSubCategory itemSubCategory = db.itemSubCategory.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(itemSubCategory);
        }

        // GET: ItemSubCategory/Create
        public ActionResult Create()
        {
            ViewBag.category = new SelectList(db.itemCategory, "categoryID", "name");
            return View();
        }

        // POST: ItemSubCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "subCategoryID,name,category")] itemSubCategory itemSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.itemSubCategory.Add(itemSubCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category = new SelectList(db.itemCategory, "categoryID", "name", itemSubCategory.category);
            return View(itemSubCategory);
        }

        // GET: ItemSubCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itemSubCategory itemSubCategory = db.itemSubCategory.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.category = new SelectList(db.itemCategory, "categoryID", "name", itemSubCategory.category);
            return View(itemSubCategory);
        }

        // POST: ItemSubCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "subCategoryID,name,category")] itemSubCategory itemSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category = new SelectList(db.itemCategory, "categoryID", "name", itemSubCategory.category);
            return View(itemSubCategory);
        }

        // GET: ItemSubCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itemSubCategory itemSubCategory = db.itemSubCategory.Find(id);
            if (itemSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(itemSubCategory);
        }

        // POST: ItemSubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            itemSubCategory itemSubCategory = db.itemSubCategory.Find(id);
            db.itemSubCategory.Remove(itemSubCategory);
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
