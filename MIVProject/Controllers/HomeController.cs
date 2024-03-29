﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIVProject.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            mivEntities db = new mivEntities();
            DateTime date = DateTime.Today;
            bool visible = true;            
            var project = db.project.Where(x => x.validTillDate >= date && x.visible == visible).Take(6);
            return View(project.ToList());
            
        }

        
        public ActionResult About()
        {            
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}