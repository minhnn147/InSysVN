using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LIB;
using Framework.EF;
using WebApplication.Code;
using WebApplication.Authorize;
using System.Configuration;

namespace WebApplication.Controllers
{
    [RoutePrefix("Users")]
    public class UsersController : BaseController
    {
        private readonly IUser _userService;
        private readonly IRole _roleService;
        private readonly ITemplate _templateService;
        private readonly IplRoleModule _roleModule;

        public UsersController()
        {
            _userService = SingletonIpl.GetInstance<IplUser>();
            _roleService = SingletonIpl.GetInstance<IplRole>();
            _templateService = SingletonIpl.GetInstance<IplTemplate>();
            _roleModule = SingletonIpl.GetInstance<IplRoleModule>();
        }

        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User },
            ActionType = new ActionType[] { ActionType.View })]
        public ActionResult Index()
        {
            ViewBag.AllRole = _roleService.GetRolesByLevel(User.RoleLevel);
            return View("Index");
        }
        public JsonResult GetDataUser(bootstrapTableParam obj, int? RoleId)
        {
            int TotalRow = 0;
            List<UserEntity> data = _userService.GetDataUsers(obj, RoleId, User.RoleLevel, ref TotalRow);
            if (data == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, data, total = TotalRow }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AutoCompletedUsers(Select2Param obj)
        {
            int total = 0;
            var data = _userService.AutoCompleteUsers(obj, ref total);
            return Json(new { success = true, results = data, total = total }, JsonRequestBehavior.AllowGet);
        }
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.Add })]
        public ActionResult Create()
        {
            ViewBag.drdRole = _roleService.GetRolesByLevel(User.RoleLevel);
            UserEntity user = new UserEntity();
            return View("CreateOrEdit", user);
        }
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.Edit })]
        public ActionResult Update(int Id)
        {
            ViewBag.drdRole = _roleService.GetRolesByLevel(User.RoleLevel);
            UserEntity user = _userService.GetUserByID(Id);
            return View("CreateOrEdit", user);
        }
        [HttpPost]
        public JsonResult SaveUser(UserEntity user, string base64Avatar, string fileName)
        {
            try
            {
                //Check userName
                UserEntity userget = _userService.GetUserByUserName(user.UserName);
                if (userget != null && (user.Id == null || user.Id != userget.Id))
                {
                    return Json(new { success = false, warning = true, status = "Tài khoản đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    user.Password = Utilities.EncodePassword(user.Password, AppSettings.PasswordHash);
                    UserEntity usere = _userService.InsertOrUpdate(user);
                    
                    if (usere != null)
                    {
                        if (fileName != null)
                        {
                            string PathServer = ControllerContext.HttpContext.Server.MapPath("~");
                            string PathFile = ConfigurationManager.AppSettings["PathUploadAvatar"] + fileName;
                            _userService.UpdateAvatar(base64Avatar, usere.Id.Value, PathServer, PathFile);
                        }
                        return Json(new { success = true, type = (user.Id == null ? 0 : 1), IsStaff = User.IsStaff }, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.View })]
        public ActionResult UploadAvatar()
        {
            return PartialView("UploadAvatar");
        }

        [HttpPost]
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.Delete })]
        public ActionResult Delete(int id)
        {
            try
            {
                string message = "";
                var get = _userService.Raw_Get(id);
                if (get == null)
                {
                    return JsonResultError("Tài khoản ko tồn tại.");
                }

                var deleteResponse = _userService.Delete(id,ref message);

                return JsonCamelCase(deleteResponse);
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }

        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.Edit, ActionType.Record })]
        public ActionResult ChangePassword(int Id)
        {
            var user = _userService.GetUserByID(Id);
            UserChangePassModel userchange = new UserChangePassModel()
            {
                UserId = user.Id.Value
            };

            return View("ChangePassword", userchange);
        }

        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.Edit, ActionType.Record })]
        [HttpPost]
        public JsonResult UpdatePassword(UserChangePassModel model)
        {
            if (!model.PasswordNew.Equals(model.PasswordReNew))
            {
                return Json(new { success = false, mess = "Xác nhận mật khẩu không khớp." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                model.PasswordCurrent = Utilities.EncodePassword(model.PasswordCurrent, AppSettings.PasswordHash);
                UserEntity acc = _userService.GetUserByID(model.UserId);
                if (!acc.Password.Equals(model.PasswordCurrent))
                {
                    return Json(new { success = false, mess = "Mật khẩu hiện tại không đúng." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.UserId = acc.Id.Value;
                    model.PasswordNew = Utilities.EncodePassword(model.PasswordNew, AppSettings.PasswordHash);
                    return Json(new { success = _userService.UpdatePassword(model) }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.Edit, ActionType.Record })]
        [HttpPost]
        public JsonResult ResetPassword(UserChangePassModel model)
        {
            if (!model.PasswordNew.Equals(model.PasswordReNew))
            {
                return Json(new { success = false, mess = "Xác nhận mật khẩu không khớp." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UserEntity acc = _userService.GetUserByID(model.UserId);
                model.UserId = acc.Id.Value;
                model.PasswordNew = Utilities.EncodePassword(model.PasswordNew, AppSettings.PasswordHash);
                return Json(new { success = _userService.UpdatePassword(model) }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}