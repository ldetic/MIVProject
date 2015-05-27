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

        [CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Index()
        {

            if (Session["type"].ToString() == "dobavljac")
            {
                string username = Session["username"].ToString();
                var supplyItem = db.supplyItem.Include(s => s.item1).Include(s => s.supplyHeader).Where(x => x.supplyHeader.supplier1.mivUser1.username == username);
                if (supplyItem != null)
                {
                    return View(supplyItem.ToList());
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }
            else
            {
                var supplyItem = db.supplyItem.Include(s => s.item1).Include(s => s.supplyHeader);
                return View(supplyItem.ToList());
            }
            
        }

        [CustomAuthorize(Roles = "administrator,referent,dobavljac,dobavljač")]
        public ActionResult Details(int? id)
        {

            if (Session["type"].ToString() == "dobavljac")
            {
                string username = Session["username"].ToString();
                if (!(db.supplyItem.Any(o => o.item == id && o.supplyHeader.supplier1.mivUser1.username == username)))
                {
                    return RedirectToAction("Index", "Home");
                }

            }

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

        [CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Create()
        {
            ViewBag.item = new SelectList(db.item, "itemID", "name");
            if (Session["type"].ToString() == "dobavljac")
            {
                string username = Session["username"].ToString();
                var supplyHeader = db.supplyHeader.Where(x => x.supplier1.mivUser1.username == username);
                ViewBag.supply = new SelectList(supplyHeader, "supplyID", "supplyID");
            }
            else
            {
                ViewBag.supply = new SelectList(db.supplyHeader, "supplyID", "supplyID");
            }
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

        [CustomAuthorize(Roles = "administrator,referent")]
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

        [CustomAuthorize(Roles = "administrator,referent")]
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
