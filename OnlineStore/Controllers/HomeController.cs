﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var loggedInUser = Session["username"];
            var useFullName = Session["fullName"];
            var role = Session["role"];

            ViewBag.Username = loggedInUser;
            ViewBag.FullName = useFullName;
            ViewBag.Role = role;
            return View();
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