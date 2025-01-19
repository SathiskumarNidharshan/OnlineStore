using OnlineStore.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Admin()
        {
            var loggedInUser = Session["username"];
            var useFullName = Session["fullName"];
            var role = Session["role"];

            // Assuming you have a context for your database models
            var db = new ApplicationDbContext(); // Adjust according to your actual context

            // Get counts for Books, Customers, Orders, and Feedbacks
            int bookCount = db.Books.Count(); // Adjust to your entity
            //int customerCount = db.Customers.Count(); // Adjust to your entity
            int orderCount = db.Orders.Count(); // Adjust to your entity
            int feedbackCount = db.Feedbacks.Count(); // Adjust to your entity

            ViewBag.Username = loggedInUser;
            ViewBag.FullName = useFullName;
            ViewBag.Role = role;
            ViewBag.BookCount = bookCount;
            //ViewBag.CustomerCount = customerCount;
            ViewBag.OrderCount = orderCount;
            ViewBag.FeedbackCount = feedbackCount;
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }


    }
}