using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Authorize;

namespace WebApplication.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }
        [UserAuthorize()]
        [HttpGet]
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}