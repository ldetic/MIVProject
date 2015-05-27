using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MIVProject;
using System.Security.Cryptography;
using System.Text;

namespace MIVProject.Controllers
{
    [CustomAuthorize(Roles = "administrator,referent")]
    public class mivUserController : Controller
    {
        private mivEntities db = new mivEntities();

        private string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }


      
        public ActionResult Index()
        {
           // var u = db.Database.SqlQuery<userType>("Select * from userType where type = 'dobavljac or type = 'dobavljač'");
          
            if (Session["type"].ToString() == "referent") 
            {
                 var mivUser = db.mivUser.Include(m => m.userType).Where(x => x.userType.type=="dobavljac" || x.type == null);
                 if (mivUser != null)
                 {
                     return View(mivUser.ToList());
                 }
                 else return RedirectToAction("Index", "Home");
               
            }
            else
            {
                 var mivUser = db.mivUser.Include(m => m.userType);
                 return View(mivUser.ToList());
            }
           
        }

       
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

        [CustomAuthorize(Roles = "administrator")]
        public ActionResult Create()
        {
            ViewBag.type = new SelectList(db.userType, "typeID", "type");
            ViewBag.userID = new SelectList(db.supplier, "mivUser", "name");
            return View();
        }

        // POST: mivUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,username,password,firstName,lastName,type,email")] mivUser mivUser)
        {
            mivUser.password = GetMD5HashData(mivUser.password.ToString());
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

        // POST: mivUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,username,password,firstName,lastName,type,email")] mivUser mivUser)
        {
            mivUser.password = GetMD5HashData(mivUser.password.ToString());
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
