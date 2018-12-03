using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebApplication.Models;
using Newtonsoft.Json;
using WebApplication.Code;
using WebApplication.Authorize;
using System.Configuration;
using System.IO;
using log4net;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(CustomPluralizedMapper<>);
            createDirectory();
            //JobScheduler.Start();
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                    CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                    newUser.Id = serializeModel.UserId;
                    newUser.FullName = serializeModel.FullName;
                    newUser.UserName = serializeModel.UserName;
                    newUser.Email = serializeModel.Email;
                    newUser.AvatarImg = serializeModel.AvatarImg;
                    newUser.WarehouseId = serializeModel.WarehouseId;
                    newUser.WarehouseName = serializeModel.WarehouseName;
                    newUser.RoleId = serializeModel.RoleId;
                    newUser.RoleLevel = serializeModel.RoleLevel;
                    HttpContext.Current.User = newUser;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                }
            }
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception lastException = Server.GetLastError();
            Log.Error(lastException);
            if (lastException == null) return;
            Response.Redirect("/Error");

        }
        protected void createDirectory()
        {
            string[] arrConfigPath = {
                "PathUploadLogo",
                "PathUploadAvatar",
                "PathUploadFileImport",
                "PathUploadFileTemplate",
                "PathSaveFileExport"
            };
            foreach(var path in arrConfigPath)
            {
                if (!Directory.Exists(Server.MapPath(ConfigurationManager.AppSettings[path])))
                {
                    Directory.CreateDirectory(Server.MapPath(ConfigurationManager.AppSettings[path]));
                }
            }
        }
    }
}
