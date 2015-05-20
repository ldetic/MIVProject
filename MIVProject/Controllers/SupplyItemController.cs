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
    public class SupplyItemController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: SupplyItem
        public ActionResult Index()
        {
            var supplyItem = db.supplyItem.Include(s => s.item1).Include(s => s.supplyHeader);
            return View(supplyItem.ToList());
        }

        // GET: SupplyItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyItem supplyItem = db.supplyItem.Find(id);
            if (supplyItem == null)
            {
                return HttpNotFound();
            }
            return View(supplyItem);
        }

        // GET: SupplyItem/Create
        public ActionResult Create()
        {
            ViewBag.item = new SelectList(db.item, "itemID", "name");
            ViewBag.supply = new SelectList(db.supplyHeader, "supplyID", "supplyID");
            return View();
        }

        // POST: SupplyItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "supply,item,itemNumber,quantity,price,quality,comment,shipDate")] supplyItem supplyItem)
        {
            if (ModelState.IsValid)
            {
                db.supplyItem.Add(supplyItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.item = new SelectList(db.item, "itemID", "name", supplyItem.item);
            ViewBag.supply = new SelectList(db.supplyHeader, "supplyID", "supplyID", supplyItem.supply);
            return View(supplyItem);
        }

        // GET: SupplyItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyItem supplyItem = db.supplyItem.Find(id);
            if (supplyItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.item = new SelectList(db.item, "itemID", "name", supplyItem.item);
            ViewBag.supply = new SelectList(db.supplyHeader, "supplyID", "supplyID", supplyItem.supply);
            return View(supplyItem);
        }

        // POST: SupplyItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "supply,item,itemNumber,quantity,price,quality,comment,shipDate")] supplyItem supplyItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplyItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.item = new SelectList(db.item, "itemID", "name", supplyItem.item);
            ViewBag.supply = new SelectList(db.supplyHeader, "supplyID", "supplyID", supplyItem.supply);
            return View(supplyItem);
        }

        // GET: SupplyItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyItem supplyItem = db.supplyItem.Find(id);
            if (supplyItem == null)
            {
                return HttpNotFound();
            }
            return View(supplyItem);
        }

        // POST: SupplyItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplyItem supplyItem = db.supplyItem.Find(id);
            db.supplyItem.Remove(supplyItem);
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
