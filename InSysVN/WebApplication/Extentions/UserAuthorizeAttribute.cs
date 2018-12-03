using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LIB;
using Framework.EF;
using LIB.RoleModule;

namespace WebApplication.Authorize
{
    public class AccesUser
    {
        public static bool checkAccess(ActionModule[] Modules, ActionType[] ActionType, CustomPrincipal acc)
        {
            IRoleModule _roleModule = SingletonIpl.GetInstance<IplRoleModule>();
            List<RoleModuleEntity> listRoleModule = _roleModule.GetDataRoleModule_ByRoleId(acc.RoleId);
            listRoleModule = listRoleModule.Where(t => Modules.Any(t1 => t.ModuleName.Contains(t1.ToString()))).ToList();
            if (listRoleModule.Count > 0)
            {
                bool check = false;
                foreach (var rolemodule in listRoleModule)
                {
                    var Listtype = rolemodule.GetType().GetProperties().Where(t => ActionType.Any(t1 => t.Name.Contains(t1.ToString()))).ToList();
                    foreach (var it in Listtype)
                    {
                        var value = rolemodule.GetType().GetProperty(it.Name).GetValue(rolemodule);
                        if ((bool)value == false) {
                            check = false;
                            break;
                        }
                        else if((bool)value == true)
                        {
                            check = true;
                        }
                    }
                    if (check == false) break;
                }
                return check;
            }
            return false;
        }
    }
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }
        public ActionType[] ActionType { get; set; }
        public ActionModule[] Modules { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var account = filterContext.HttpContext.User as CustomPrincipal;
                Modules = Modules ?? new ActionModule[] { };
                ActionType = ActionType ?? new ActionType[] { };
                if (Modules.Length == 0 && ActionType.Length == 0)
                {
                    base.OnAuthorization(filterContext);
                    return;
                }
                var checkAccess = AccesUser.checkAccess(Modules, ActionType, account);

                if (!checkAccess)
                {
                    if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                    {
                        //var mess = "Yêu cầu trái phép <br> Bạn không có quyền truy cập tài nguyên này.<br>Để được truy cập, vui lòng liên hệ với quản trị viên hệ thống của bạn để được cấp phép.";
                        //filterContext.Result = new JsonResult()
                        //{
                        //    Data = new
                        //    {
                        //        Success = false,
                        //        Error = mess,
                        //        Message = mess
                        //    },
                        //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        //};
                        var viewResult = new PartialViewResult
                        {
                            ViewName = "~/Views/Unauthorised/Index.cshtml",
                        };
                        filterContext.Result = viewResult;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Unauthorised", action = "Index" }));
                    }
                }
                //}
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }

        }
    }
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (Roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsSupperAdmin
        {
            get { return RoleLevel == (int)enumRoleLevel.Super_Admin ? true : false; }
        }
        public bool IsAdmin
        {
            get { return RoleLevel == (int)enumRoleLevel.Admin ? true : false; }
        }
        public bool IsManager//quản lý cửa hàng
        {
            get { return RoleLevel == (int)enumRoleLevel.Manager ? true : false; }
        }
        public bool IsStaff//bán hàng
        {
            get { return RoleLevel == (int)enumRoleLevel.Staff ? true : false; }
        }
        public bool IsAccounting
        {
            get { return RoleLevel == (int)enumRoleLevel.Accounting ? true : false; }
        }
        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
        public string AvatarImg { get; set; }
        public int? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int RoleId { get; set; }
        public int RoleLevel { get; set; }
    }
    public enum ActionModule
    {
        Order,
        OrderPromotion,
        Category,
        Product,
        Customer,
        Return,
        Report,
        Template,
        User,
        Role,
        ShowRoom,
        SyncSetting
    }
    public enum enumRoleLevel
    {
        Super_Admin = 0,
        Admin = 1,
        Manager = 2,
        Staff = 4,
        Accounting = 3
    }
    public enum ActionType
    {
        Add,
        Edit,
        View ,
        Delete,
        Import,
        Export,
        Upload,
        Publish,
        Report,
        Sync,
        Accept,
        Cancel,
        Record
    }
}