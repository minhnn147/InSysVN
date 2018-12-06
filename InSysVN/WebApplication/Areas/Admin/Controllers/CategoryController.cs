using Framework.EF;
using LIB;
using LIB.Product;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private ICategory _categoryService;
        private readonly IProduct _productService;
        private readonly IUser _userService;

        public CategoryController()
        {
            _categoryService = SingletonIpl.GetInstance<IplCategory>();
            _productService = SingletonIpl.GetInstance<IplProduct>();
            _userService = SingletonIpl.GetInstance<IplUser>();
        }
        // GET: Admin/Category
        public ActionResult Index()
        {
            List<CategoryEntity> list = _categoryService.GetAllWithLevel("");
            return View();
        }
        public JsonResult GetCateData(string txtSearch)
        {
            List<CategoryEntity> lstcate = _categoryService.GetAllWithLevel(txtSearch);
            int totalcount = lstcate.Count();
            return Json(new { status = true, total = totalcount, rows = lstcate }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            CategoryEntity cates = new CategoryEntity();
            return View("Create_Edit", cates);
        }
        public ActionResult Eidt(int Id)
        {
            CategoryEntity cates = _categoryService.GetByID(Id);
            return View("Create_Edit", cates);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult InsertCate(CategoryEntity category, string base64Avatar, string fileName)
        {
            try
            {
                bool categet = _categoryService.CheckExistCate(category.Name);
                if (categet && category.Id == null)
                {
                    return Json(new { success = false, warning = true, status = "Danh mục đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    category.ImagePath = fileName;
                    CategoryEntity res = _categoryService.Insert_Update(category);
                    if (res != null)
                    {
                        if (fileName != null)
                        {
                            //string PathServer = ControllerContext.HttpContext.Server.MapPath("~");
                            string PathServer = ConfigurationManager.AppSettings["PathUploadServer"];
                            string PathFile = ConfigurationManager.AppSettings["PathUploadCateImage"] + fileName;
                            _categoryService.UpdateCateImage(base64Avatar, res.Id.Value, PathServer, PathFile);
                        }
                        return Json(new { success = true, type = (category.Id == null ? 0 : 1) }, JsonRequestBehavior.AllowGet);
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
        public JsonResult UpdateIsActive(int cateId)
        {
            bool flag = _categoryService.UpdateIsActive(cateId);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_Update(CategoryEntity category)
        {
            try
            {
                return Json(new { success = _categoryService.Insert_Update(category) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, message = ex }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Delete(int Id)
        {
            try
            {
                return Json(new { success = _categoryService.DeleteById(Id) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}