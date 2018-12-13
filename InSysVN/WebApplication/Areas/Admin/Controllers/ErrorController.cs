using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Admin.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Admin/Error
        public ActionResult Index()
        {
            return View();
        }
    }
}