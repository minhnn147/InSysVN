using Framework.EF;
using LIB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Authorize;


namespace WebApplication.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private INews _newsService;

        public NewsController()
        {
            _newsService = SingletonIpl.GetInstance<IplNews>();
        }

        // GET: Admin/News
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetNewsData()
        {
            List<NewsEntity> lstcate = _newsService.GetAllNews();
            if (lstcate != null)
            {
                int totalcount = lstcate.Count();
                return Json(new { status = true, total = totalcount, rows = lstcate }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.Add })]
        public ActionResult Create()
        {
            NewsEntity user = new NewsEntity();
            return View("CreateOrEdit", user);
        }
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.User }, ActionType = new ActionType[] { ActionType.Edit })]
        public ActionResult Update(int Id)
        {
            NewsEntity user = _newsService.GetNewsByID(Id);
            return View("CreateOrEdit", user);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult InsertNews(NewsEntity newsentity, string base64Avatar, string fileName)
        {
            try
            {
                bool checknews = _newsService.CheckByTitle(newsentity.Title);
                if (checknews && newsentity.ID == null)
                {
                    return Json(new { success = false, warning = true, status = "Danh mục đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    newsentity.ImageTitle = fileName;
                    NewsEntity res = _newsService.Insert_Update(newsentity);
                    if (res != null)
                    {
                        if (fileName != null)
                        {
                            string PathServer = ControllerContext.HttpContext.Server.MapPath("~");
                            string PathFile = ConfigurationManager.AppSettings["PathUploadNewImage"] + fileName;
                            _newsService.UpdateNewsImage(base64Avatar, res.ID.Value, PathServer, PathFile);
                        }
                        return Json(new { success = true, type = (res.ID == null ? 0 : 1) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateIsActive(int newsId)
        {
            bool flag = _newsService.UpdateIsActive(newsId);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int Id)
        {
            try
            {
                return Json(new { success = _newsService.DeleteByNewsId(Id) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}