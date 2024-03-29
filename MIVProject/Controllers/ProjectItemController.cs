﻿using System;
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
    public class projectItemController : Controller
    {
        private mivEntities db = new mivEntities();

        [CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Index()
        {
            var projectItem = db.projectItem.Include(p => p.item1).Include(p => p.project1);
            return View(projectItem.ToList());
        }

        [CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectItem projectItem = db.projectItem.Find(id);
            if (projectItem == null)
            {
                return HttpNotFound();
            }
            return View(projectItem);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Create()
        {
            ViewBag.item = new SelectList(db.item, "itemID", "name");
            ViewBag.project = new SelectList(db.project, "id", "name");
            return View();
        }

        // POST: projectItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate,item, projectItemID")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {

                db.Database.ExecuteSqlCommand("insert into projectItem(quantity,price,quality,description,project,comment,shipDate,item) values(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)",
                    projectItem.quantity, projectItem.price, projectItem.quality, projectItem.description, projectItem.project, projectItem.comment, projectItem.shipDate, projectItem.item);
                //db.projectItem.Add(projectItem);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.item = new SelectList(db.item, "itemID", "name", projectItem.item);
            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        // POST: projectItem/CreateViaAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public String CreateViaAjax([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate,item")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("insert into projectItem(quantity,price,quality,description,project,comment,shipDate,item) values(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)",
                    projectItem.quantity, projectItem.price, projectItem.quality, projectItem.description, projectItem.project, projectItem.comment, projectItem.shipDate, projectItem.item);
                //db.projectItem.Add(projectItem);
                //db.SaveChanges();
                return "OK";
            }
            return "ERROR";
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectItem projectItem = db.projectItem.Find(id);
            if (projectItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.item = new SelectList(db.item, "itemID", "name", projectItem.item);
            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        // POST: projectItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate,item")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("update projectItem set quantity=@p0,price=@p1,quality=@p2,description=@p3,project=@p4,comment=@p5,shipDate=@p6,item=@p7 where projectItemID = @p8",
                   projectItem.quantity, projectItem.price, projectItem.quality, projectItem.description, projectItem.project, projectItem.comment, projectItem.shipDate, projectItem.item, projectItem.projectItemID);

                //db.Entry(projectItem).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.item = new SelectList(db.item, "itemID", "name", projectItem.item);
            ViewBag.project = new SelectList(db.project, "id", "name", projectItem.project);
            return View(projectItem);
        }

        // POST: projectItem/EditViaAjax/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public String EditViaAjax([Bind(Include = "quantity,price,quality,description,projectPosition,project,comment,shipDate,item")] projectItem projectItem)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("update projectItem set quantity=@p0,price=@p1,quality=@p2,description=@p3,project=@p4,comment=@p5,shipDate=@p6,item=@p7 where projectItemID = @p8",
                   projectItem.quantity, projectItem.price, projectItem.quality, projectItem.description, projectItem.project, projectItem.comment, projectItem.shipDate, projectItem.item, projectItem.projectItemID);

                return "OK";
            }
            return "ERROR";
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectItem projectItem = db.projectItem.Find(id);
            if (projectItem == null)
            {
                return HttpNotFound();
            }
            return View(projectItem);
        }

        
        // POST: projectItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            projectItem projectItem = db.projectItem.Find(id);
            db.projectItem.Remove(projectItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: projectItem/DeleteViaAjax/5
        [HttpPost, ActionName("DeleteViaAjax")]
        [ValidateAntiForgeryToken]
        public String DeleteViaAjax(int id)
        {
            projectItem projectItem = db.projectItem.Find(id);
            db.projectItem.Remove(projectItem);
            db.SaveChanges();
            return "OK";
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
