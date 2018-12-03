using LIB.DataRequests;
using LIB.Model;
using LibCore.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Authorize;
using WebApplication.Code;

namespace WebApplication.Controllers
{
    //[UserAuthorize]
    public class BaseController : Controller
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseController()
        {

        }
        
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            return base.BeginExecuteCore(callback, state);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = Request.RequestContext.RouteData.Values["Controller"].ToString();
            var actionName = Request.RequestContext.RouteData.Values["Action"].ToString();
            //var sphereId = 0;
            //if (Enum.IsDefined(typeof(EnumValues.Sphere), controllerName))
            //{
            //    sphereId = (int)Enum.Parse(typeof(EnumValues.Sphere), controllerName);
            //}
            //filterContext.Controller.ViewBag.SphereID = sphereId;
            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        // GET: Base
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        protected PagingRequest GetPagingMessage(NameValueCollection queries)
        {
            var limit = Utility.StringToInt(queries["limit"]);
            var offset = Utility.StringToInt(queries["offset"]);
            var sort = queries["sort"] ?? "a.Id";
            var order = queries["order"];
            return new PagingRequest
            {
                PageIndex = offset / limit + 1,
                PageSize = limit,
                Sort = sort + " " + order,
            };
        }

        protected JsonCamelCaseResult JsonResultSuccess(object jsonObject, string Message = "",bool Success = true, bool allowGet = true)
        {
            return new JsonCamelCaseResult(new
            {
                Success = Success,
                Message = !string.IsNullOrEmpty(Message) ? Message : "Success",
                Data = jsonObject
            }, allowGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet);
        }
        protected JsonCamelCaseResult JsonResultError(ModelStateDictionary modelState, bool allowGet = true)
        {
            var errors = ModelState.Select(x => x.Value.Errors)
                              .Where(y => y.Count > 0).Select(e => e.First().ErrorMessage)
                              .ToList();
            return JsonResultError(string.Join("<br>", errors), allowGet);
        }
        protected JsonCamelCaseResult JsonResultError(Exception ex, bool allowGet = true)
        {
            return JsonResultError(ex.Message, allowGet);
        }
        protected JsonCamelCaseResult JsonResultError(string Message, bool allowGet = true)
        {
            return new JsonCamelCaseResult(new
            {
                Success = false,
                Message = Message,
            }, allowGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet);
        }
        protected JsonCamelCaseResult JsonCamelCase(object jsonObject, bool allowGet = true)
        {
            return new JsonCamelCaseResult(jsonObject, allowGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet);
        }


        #region Upload
        protected string GetFullRootUrl()
        {
            return string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            //HttpRequest request = System.Web.HttpContext.Current.Request;
            //return request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath, String.Empty);
        }
        #endregion
    }
}