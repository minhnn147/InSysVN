using Framework.EF;
using LIB;
using LIB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class ModulesController : BaseController
    {
        private readonly IModule _moduleService;

        // GET: Modules
        public ModulesController()
        {
            _moduleService = SingletonIpl.GetInstance<IplModule>();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetModuleByRoleId(int RoleId)
        {
            return Json(new { success = true, data = _moduleService.GetListModuleByRoleId(RoleId) }, JsonRequestBehavior.AllowGet);
        }
    }
}