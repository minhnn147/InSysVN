using Framework.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LIB;
using OfficeOpenXml;
using WebApplication.Authorize;

namespace WebApplication.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController()
        {
        }
        [UserAuthorize()]
        [HttpGet]
        public ActionResult Index()
        {
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

        class Customer
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }
    }
}