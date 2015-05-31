using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MIVProject;
using System.Configuration;
using System.Net.Mail;
using Resources;

namespace MIVProject.Controllers
{

    public class SupplyHeaderController : Controller
    {
        private mivEntities db = new mivEntities();

        public int SendEmail(string email, string body, string subject)
        {
            try
            {
                var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
                var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();


                MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(email, ""));
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;

                var client = new SmtpClient();
                client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
                client.Host = smtpHost;
                client.EnableSsl = true;
                client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
                client.Send(message);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        [CustomAuthorize(Roles = "administrator,referent,dobavljac")]
        public ActionResult Index()
        {
            if (Session["type"].ToString() == "dobavljac")
            {
                string username = Session["username"].ToString();
                var supplyHeader = db.supplyHeader.Include(s => s.currency1).Include(s => s.deliveryMethod1).Include(s => s.paymentMethod1).Include(s => s.project1).Include(s => s.supplier1).Include(s => s.supplyStatus).Where(x => x.supplier1.mivUser1.username == username);
                if (supplyHeader != null)
                {
                    return View(supplyHeader.ToList());
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }
            else
            {
                var supplyHeader = db.supplyHeader.Include(s => s.currency1).Include(s => s.deliveryMethod1).Include(s => s.paymentMethod1).Include(s => s.project1).Include(s => s.supplier1).Include(s => s.supplyStatus);
                return View(supplyHeader.ToList());
            }
        }

        [CustomAuthorize(Roles = "administrator,referent,dobavljac")]
        public ActionResult Details(int? id)
        {
            if (Session["type"].ToString() == "dobavljac")
            {
                string username = Session["username"].ToString();
                if (!(db.supplyHeader.Any(o => o.supplyID == id && o.supplier1.mivUser1.username == username)))
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyHeader supplyHeader = db.supplyHeader.Find(id);
            if (supplyHeader == null)
            {
                return HttpNotFound();
            }
            IQueryable<supplyItem> supplyItems = db.supplyItem.Where(a => a.supply == id);
            ViewBag.supplyItems = supplyItems;
            if (supplyHeader == null)
            {
                return HttpNotFound();
            }


            return View(supplyHeader);
        }

        [CustomAuthorize(Roles = "administrator,referent,dobavljač,dobavljac")]
        public ActionResult Create()
        {
            ViewBag.currency = new SelectList(db.currency, "currencyID", "name");
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name");
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name");
            ViewBag.project = new SelectList(db.project, "id", "name");

            ViewBag.status = new SelectList(db.supplyStatus, "statusID", "name");

            if (Session["type"].ToString() == "dobavljač" || Session["type"].ToString() == "dobavljac")
            {
                string username = Session["username"].ToString();
                var supplier = db.supplier.Where(x => x.mivUser1.username == username);
                //ViewBag.supply = new SelectList(supplier, "supplyID", "supplyID");
                ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name");
            }
            else
            {
                ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name");
            }

            return View();
        }

        // POST: SupplyHeader/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "supplyID,paymentMethod,deliveryMethod,paymentDate,deliveryDate,supplier,date,project,status,currency")] supplyHeader supplyHeader)
        {
            if (Session["type"].ToString() == "dobavljač" || Session["type"].ToString() == "dobavljac")
            {
                supplyHeader.supplier = Convert.ToInt32(Session["userID"]);
                var supplyStatus = db.supplyStatus.Where(x => x.name == "U izradi");
                supplyHeader.status = supplyStatus.First().statusID;
                //supplyHeader.supplier1.mivUser1.username = Session["username"].ToString();
            }
            if (ModelState.IsValid)
            {
                db.supplyHeader.Add(supplyHeader);
                db.SaveChanges();
                var supplier = db.supplier.Where(x => x.mivUser == supplyHeader.supplier);
                string body = Local.NewSupply + " " + supplier.First().name;
                string subject = Local.NewSupply + " " + supplier.First().name;
                SendEmail("ljdetic@gmail.com", body, subject);
                return RedirectToAction("Index");
            }

            ViewBag.currency = new SelectList(db.currency, "currencyID", "name", supplyHeader.currency);
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", supplyHeader.deliveryMethod);
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", supplyHeader.paymentMethod);
            ViewBag.project = new SelectList(db.project, "id", "name", supplyHeader.project);
            ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name", supplyHeader.supplier);
            ViewBag.status = new SelectList(db.supplyStatus, "statusID", "name", supplyHeader.status);

            return View(supplyHeader);
        }

        // POST: SupplyHeader/CreateViaAJax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public String CreateViaAjax([Bind(Include = "supplyID,paymentMethod,deliveryMethod,paymentDate,deliveryDate,supplier,date,project,status,currency")] supplyHeader supplyHeader)
        {
            if (Session["type"].ToString() == "dobavljač" || Session["type"].ToString() == "dobavljac")
            {
                supplyHeader.supplier = Convert.ToInt32(Session["userID"]);
            }
            if (ModelState.IsValid)
            {
                db.supplyHeader.Add(supplyHeader);
                db.SaveChanges();
                var supplier = db.supplier.Where(x => x.mivUser == supplyHeader.supplier);
                string body = Local.NewSupply + " " + supplier.First().name;
                string subject = Local.NewSupply + " " + supplier.First().name;
                SendEmail("ljdetic@gmail.com", body, subject);
                return supplyHeader.supplyID.ToString();
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
            supplyHeader supplyHeader = db.supplyHeader.Find(id);
            if (supplyHeader == null)
            {
                return HttpNotFound();
            }

           
                ViewBag.currency = new SelectList(db.currency, "currencyID", "name", supplyHeader.currency);
                ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", supplyHeader.deliveryMethod);
                ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", supplyHeader.paymentMethod);
                ViewBag.project = new SelectList(db.project, "id", "name", supplyHeader.project);
                ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name", supplyHeader.supplier);
           


            ViewBag.status = new SelectList(db.supplyStatus, "statusID", "name", supplyHeader.status);
            return View(supplyHeader);
        }

        // POST: SupplyHeader/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "supplyID,paymentMethod,deliveryMethod,paymentDate,deliveryDate,supplier,date,project,status,currency")] supplyHeader supplyHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplyHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.currency = new SelectList(db.currency, "currencyID", "name", supplyHeader.currency);
            ViewBag.deliveryMethod = new SelectList(db.deliveryMethod, "deliveryID", "name", supplyHeader.deliveryMethod);
            ViewBag.paymentMethod = new SelectList(db.paymentMethod, "paymentID", "name", supplyHeader.paymentMethod);
            ViewBag.project = new SelectList(db.project, "id", "name", supplyHeader.project);
            ViewBag.supplier = new SelectList(db.supplier, "mivUser", "name", supplyHeader.supplier);
            ViewBag.status = new SelectList(db.supplyStatus, "statusID", "name", supplyHeader.status);
            return View(supplyHeader);
        }

        [CustomAuthorize(Roles = "administrator,referent")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplyHeader supplyHeader = db.supplyHeader.Find(id);
            if (supplyHeader == null)
            {
                return HttpNotFound();
            }
            return View(supplyHeader);
        }

        // POST: SupplyHeader/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Database.ExecuteSqlCommand("Delete from supplyItem where supply = @p0", id);
            
            supplyHeader supplyHeader = db.supplyHeader.Find(id);
            db.supplyHeader.Remove(supplyHeader);
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
