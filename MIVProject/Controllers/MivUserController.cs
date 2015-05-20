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
    public class MivUserController : Controller
    {
        private mivEntities db = new mivEntities();

        // GET: MivUser
        public ActionResult Index()
        {
            var mivUser = db.mivUser.Include(m => m.userType).Include(m => m.supplier);
            return View(mivUser.ToList());
        }

        // GET: MivUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mivUser mivUser = db.mivUser.Find(id);
            if (mivUser == null)
            {
                return HttpNotFound();
            }
            return View(mivUser);
        }

        // GET: MivUser/Create
        public ActionResult Create()
        {
            ViewBag.type = new SelectList(db.userType, "typeID", "type");
            ViewBag.userID = new SelectList(db.supplier, "mivUser", "name");
            return View();
        }

        // POST: MivUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,username,password,type,firstName,lastName")] mivUser mivUser)
        {
            if (ModelState.IsValid)
            {
                db.mivUser.Add(mivUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.type = new SelectList(db.userType, "typeID", "type", mivUser.type);
            ViewBag.userID = new SelectList(db.supplier, "mivUser", "name", mivUser.userID);
            return View(mivUser);
        }

        // GET: MivUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mivUser mivUser = db.mivUser.Find(id);
            if (mivUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.type = new SelectList(db.userType, "typeID", "type", mivUser.type);
            ViewBag.userID = new SelectList(db.supplier, "mivUser", "name", mivUser.userID);
            return View(mivUser);
        }

        // POST: MivUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,username,password,type,firstName,lastName")] mivUser mivUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mivUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.type = new SelectList(db.userType, "typeID", "type", mivUser.type);
            ViewBag.userID = new SelectList(db.supplier, "mivUser", "name", mivUser.userID);
            return View(mivUser);
        }

        // GET: MivUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mivUser mivUser = db.mivUser.Find(id);
            if (mivUser == null)
            {
                return HttpNotFound();
            }
            return View(mivUser);
        }

        // POST: MivUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mivUser mivUser = db.mivUser.Find(id);
            db.mivUser.Remove(mivUser);
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
