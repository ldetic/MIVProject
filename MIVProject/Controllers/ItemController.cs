using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MIVProject;
using Newtonsoft.Json;
using System.Dynamic;

namespace MIVProject.Controllers
{
    public class ItemController : Controller
    {
        private mivEntities db = new mivEntities();
        

        // GET: Item
        public ActionResult Index()
        {
            var item = db.item.Include(i => i.itemSubCategory);
            return View(item.ToList());
        }

        // GET: Item/AllJSON
        [HttpGet, ActionName("AllJSON")]
        public String AllJSON()
        {
            var item = db.item.Include(i => i.itemSubCategory);
            List<Object> items = new List<object>();
            dynamic itemObject = new ExpandoObject();
            foreach (var it in item)
            {
                

                var obj = new List<KeyValuePair<string, string>>();
                obj.Add(new KeyValuePair<string, string>("id", it.itemID.ToString()));
                itemObject.id = it.itemID;
                if (!String.IsNullOrEmpty(it.name))
                {
                    itemObject.name = it.name;
                } else
                {
                    itemObject.name = "";
                }
                if (!String.IsNullOrEmpty(it.description))
                {
                    //obj.Add(new KeyValuePair<string, string>("description", it.description.ToString()));
                    itemObject.description = it.description;
                } else
                {
                    itemObject.description = "";
                }

                //obj.Add(new KeyValuePair<string, string>("quantity", it.quantity.ToString()));
                itemObject.quantity = it.quantity;

                if (!String.IsNullOrEmpty(it.unitOfMeasure))
                {
                    //obj.Add(new KeyValuePair<string, string>("unitOfMeasuer", it.unitOfMeasure.ToString()));
                    itemObject.unitOfMeasure = it.unitOfMeasure;
                } else
                {
                    itemObject.unitOfMeasure = "";
                }
                if (!String.IsNullOrEmpty(it.itemSubCategory.name))
                {
                    //obj.Add(new KeyValuePair<string, string>("subCategory", it.itemSubCategory.name.ToString()));
                    itemObject.subCategory = it.itemSubCategory.name;
                } else
                {
                    itemObject.subCategory = "";
                }
                items.Add(JsonConvert.SerializeObject(itemObject));
                
            }
        
            //return Json(item, JsonRequestBehavior.AllowGet);

            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };           
            return JsonConvert.SerializeObject(items, Formatting.Indented, serializerSettings);
        }

        // GET: Item/Details/5
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

        // GET: Item/Create
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

        // GET: Item/Edit/5
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

        // GET: Item/Delete/5
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
            try {
                item item = db.item.Find(id);
                db.item.Remove(item);
                db.SaveChanges();
                return "OK";
            } catch(Exception ex)
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
