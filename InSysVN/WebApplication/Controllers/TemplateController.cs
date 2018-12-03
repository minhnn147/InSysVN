using Framework.EF;
using LIB;
using LIB.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Authorize;

namespace WebApplication.Controllers
{
    public class TemplateController : BaseController
    {
        private readonly ITemplate _template;
        public TemplateController()
        {
            _template = SingletonIpl.GetInstance<IplTemplate>();
        }
        // GET: Template
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetData()
        {
            List<TemplateEntity> data = _template.GetData();
            if (data == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
        }
    }
}