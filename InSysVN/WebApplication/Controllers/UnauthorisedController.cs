using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Authorize;

namespace WebApplication.Controllers
{
    public class UnauthorisedController : Controller
    {
        // GET: Unauthorised
        [UserAuthorize()]
        public ActionResult Index()
        {
            return View();
        }
    }
}