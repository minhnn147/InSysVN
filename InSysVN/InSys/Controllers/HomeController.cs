using Framework.EF;
using LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InSys.Controllers
{
    public class HomeController : Controller
    {
        private ICategory _cateRepo;
        public HomeController()
        {
            _cateRepo = SingletonIpl.GetInstance<IplCategory>();
        }
        public ActionResult Index()
        {
            ViewBag.Category = _cateRepo.GetAllData();
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