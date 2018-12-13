using Framework.EF;
using LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsysVina.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategory category;
        public HomeController()
        {
            category = SingletonIpl.GetInstance<IplCategory>();
        }
        public ActionResult Index()
        {
            ViewBag.Category = category.GetAllData().Where(x => x.isActive == true).ToList();
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