using Framework.EF;
using LIB;
using LIB.Model;
using LIB.RoleModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebApplication.Authorize;

namespace WebApplication.Controllers
{
    public class RoleModuleController : BaseController
    {
        private readonly IModule _moduleService;
        private readonly IRole _roleService;
        private readonly IplRoleModule _roleModule;
        // GET: Right
        public RoleModuleController()
        {
            _moduleService = SingletonIpl.GetInstance<IplModule>();
            _roleService = SingletonIpl.GetInstance<IplRole>();
            _roleModule = SingletonIpl.GetInstance<IplRoleModule>();
        }
    
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.Role }, ActionType = new ActionType[] { ActionType.Report })]
        public ActionResult Index()
        {
            ViewBag.Roles = _roleService.GetRolesByLevel(User.RoleLevel);
            ViewBag.Modules = _moduleService.GetDataModule();
            return View("Index");
        }
        public ActionResult GetDataRoleModule(long RoleId)
        {
            List<RoleModuleEntity> role_module = _roleModule.GetDataRoleModule_ByRoleId(RoleId);
            return PartialView("_TableRoleModule", role_module);
        }
        public JsonResult Save(List<RoleModuleEntity> list)
        {
            try
            {
                string xml = XMLHelper.SerializeXML<List<RoleModuleEntity>>(list);
                bool kq = _roleModule.Save(xml);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddModuleToRole(int RoleId, List<int?> ModulesId)
        {
            return Json(new { success = _roleModule.AddModuleToRole(RoleId,String.Join(",", ModulesId.ToArray())) },JsonRequestBehavior.AllowGet);
        }

        #region Mpdule
        [HttpPost]
        public ActionResult ModuleSaveModuleAndOrder(List<ModuleEntity> datas)
        {
            try
            {
                var xml = XMLHelper.SerializeXML<List<ModuleEntity>>(datas);
                _moduleService.UpdateModuleIdAndSort(xml);
                return JsonResultSuccess(true);
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }

        [HttpPost]
        public ActionResult ModuleDelete(int Id)
        {
            try
            {
                return JsonResultSuccess(_moduleService.Raw_Delete(Id));
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }
        [HttpPost]
        public ActionResult ModuleCreate(ModuleEntity model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return JsonResultError(ModelState);
                }
                var get = _moduleService.Raw_Get(model.Id);

                var columnIgnore = new List<string>() { "ModuleId", "Sorting" };

                if (get != null)
                {
                    _moduleService.Raw_Update(model, Columns: columnIgnore, IgnoreOrSave: true);
                }
                else
                {
                    model.Sorting = 1000;
                    _moduleService.Raw_Insert(model);
                }

                return JsonResultSuccess(true);
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }

        [HttpPost]
        public ActionResult ModuleGetListTreeView()
        {
            try
            {
                var list = _moduleService.ModuleGetListTreeView();
                //var list = _roleModule.GetAll().ToList();
                return JsonResultSuccess(list);
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }
        #endregion

        #region Role
        [HttpGet]
        public ActionResult RoleList()
        {
            try
            {
                var list = _roleService.Raw_GetAll();
                //var list = _roleModule.GetAll().ToList();
                return JsonResultSuccess(list);
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }
        [HttpPost]
        public ActionResult RoleCreate(RoleEntity model)
        {
            try
            {
                var get = _roleService.Raw_Get(model.Id);
                if (get == null)
                {
                    _roleService.Raw_Insert(model);
                }
                else
                {
                    _roleService.Raw_Update(model);
                }

                return JsonResultSuccess(1);
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }

        [HttpPost]
        public ActionResult RoleDelete(int Id)
        {
            try
            {
                return JsonResultSuccess(_roleService.Raw_Delete(Id));
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }

        [HttpGet]
        public ActionResult RoleManager()
        {
            return View("RoleManager");
        }
        [HttpPost]
        public ActionResult GetRoleModule(int roleId)
        {
            return JsonResultError("");
            //try
            //{
            //    var allRole = _roleModule.ListRoleModuleByRole(roleId);
            //    var list = allRole.Where(x => x.RoleId == roleId);
            //    return JsonResultSuccess(list.ToList());
            //}
            //catch (Exception ex)
            //{
            //    return JsonResultError(ex);
            //}
        }
        [HttpPost]
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.Role }, ActionType = new ActionType[] { ActionType.Record })]
        public ActionResult EditRightModule(int roleId, int moduleId, bool status, string name)
        {
            try
            {
                var retval = _roleModule.UpdateRoleModule(roleId, moduleId, status, name);
                return JsonResultSuccess(null, "", retval);
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }
        #endregion
    }
}