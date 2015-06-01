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

    public class supplierController : Controller
    {
        private mivEntities db = new mivEntities();

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Index()
        {
            var supplier = db.supplier.Include(s => s.mivUser1).Include(s => s.supplierCategory);
            return View(supplier.ToList());
        }

        [CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Details(int? id)
        {
            if (Session["type"].ToString() == "dobavljac")
            {
                string username = Session["username"].ToString();
                if (!(db.supplier.Any(o => o.mivUser == id && o.mivUser1.username == username)))
                {
                    return RedirectToAction("Index", "Home");
                }

            }

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

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Create()
        {
            ViewBag.mivUser = new SelectList(db.mivUser, "userID", "username");
            ViewBag.category = new SelectList(db.supplierCategory, "id", "name");
            return View();
        }

        // POST: supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mivUser,name,phone,OIB,category")] supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.supplier.Add(supplier);
                db.SaveChanges();

                string username = Session["username"].ToString();
                DateTime date = DateTime.Now;
                string msg = "Supplier created " + supplier.name + " id:" + supplier.mivUser;
                int userID = (int)Session["userID"];
                db.Database.ExecuteSqlCommand("Insert into logs values(@p0, @p1, @p2, @p3 )", userID, username, msg, date);

                return RedirectToAction("Index");
            }

            ViewBag.mivUser = new SelectList(db.mivUser, "userID", "username", supplier.mivUser);
            ViewBag.category = new SelectList(db.supplierCategory, "id", "name", supplier.category);
            return View(supplier);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
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

        // POST: supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mivUser,name,phone,OIB,category")] supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();

                string username = Session["username"].ToString();
                DateTime date = DateTime.Now;
                string msg = "Supplier edited " + supplier.name + " id:" + supplier.mivUser;
                int userID = (int)Session["userID"];
                db.Database.ExecuteSqlCommand("Insert into logs values(@p0, @p1, @p2, @p3 )", userID, username, msg, date);

                return RedirectToAction("Index");
            }
            ViewBag.mivUser = new SelectList(db.mivUser, "userID", "username", supplier.mivUser);
            ViewBag.category = new SelectList(db.supplierCategory, "id", "name", supplier.category);
            return View(supplier);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
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

        // POST: supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplier supplier = db.supplier.Find(id);
            try
            {
                db.supplier.Remove(supplier);
                db.SaveChanges();

                string username = Session["username"].ToString();
                DateTime date = DateTime.Now;
                string msg = "Supplier removed " + supplier.name + " id:" + supplier.mivUser;
                int userID = (int)Session["userID"];
                db.Database.ExecuteSqlCommand("Insert into logs values(@p0, @p1, @p2, @p3 )", userID, username, msg, date);
            }
            catch
            {
                return RedirectToAction("Index");
            }
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
