using Framework.EF;
using LIB;
using LIB.Product;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Authorize;
using OfficeOpenXml;
using static LIB.ExcelExtension;
namespace WebApplication.Controllers
{
    public class CategoryController : BaseController
    {

        private ICategory _categoryService;
        private IProduct _productService;
        private IUser _userService;

        public CategoryController()
        {
            _categoryService = SingletonIpl.GetInstance<IplCategory>();
            _productService = SingletonIpl.GetInstance<IplProduct>();
            _userService = SingletonIpl.GetInstance<IplUser>();
        }
        public ActionResult Index()
        {
            List<CategoryEntity> list = _categoryService.GetAllWithLevel("");
            var data = getChildList(list, null, "");
            ViewBag.listCategory = data;
            return View();
        }
        public JsonResult GetCateData(string txtSearch)
        {
            List<CategoryEntity> lstcate = _categoryService.GetAllWithLevel(txtSearch);
            var totalcount = lstcate.Count();
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
                var categet = _categoryService.CheckExistCate(category.Name);
                if (categet && category.Id == null)
                {
                    return Json(new { success = false, warning = true, status = "Danh mục đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    category.ImagePath = fileName;
                    CategoryEntity res = _categoryService.Insert_Update(category);
                    if (res!= null)
                    {
                        if (fileName != null)
                        {
                            //string PathServer = ControllerContext.HttpContext.Server.MapPath("~");
                            string PathServer = ConfigurationManager.AppSettings["PathUploadServer"];
                            string PathFile = ConfigurationManager.AppSettings["PathUploadCateImage"] + fileName;
                            _categoryService.UpdateCateImage(base64Avatar,res.Id.Value, PathServer, PathFile);
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




        //------bỏ

        public List<CategoryEntity> getChildList(List<CategoryEntity> data, int? IdParent, string space)
        {
            List<CategoryEntity> con = new List<CategoryEntity>();
            //foreach (var itemParent in data)
            //{
            //    if (itemParent.ParentId == IdParent)
            //    {
            //        con.Add(itemParent);
            //        data = data.Where(m => m.Id != itemParent.Id).ToList();
            //        List<CategoryEntity> dataChild = getChildList(data, itemParent.Id.Value, space);
            //        foreach (var itemChild in dataChild)
            //        {
            //            itemChild.Name = space + itemChild.Name;
            //            con.Add(itemChild);
            //        }
            //    }
            //}
            return con;
        }

        public JsonResult GetAllWithLevel(string txtSearch)
        {
            try
            {
                List<CategoryEntity> data = _categoryService.GetAllWithLevel(txtSearch);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDataWithPage(Select2Param obj)
        {
            int total = 0;
            var data = _categoryService.GetDataWithPage(obj, ref total);
            return Json(new { success = true, results = data, total = total }, JsonRequestBehavior.AllowGet);

        }
        //public ActionResult Create()
        //{
        //    List<CategoryEntity> list = _categoryService.GetAllWithLevel("");
        //    if (list == null)
        //    {
        //        ViewBag.Categories = new List<CategoryEntity>();
        //        return PartialView("_Create_Edit", new CategoryEntity());
        //    }
        //    else
        //    {
        //        var data = getChildList(list, null, "--- ");
        //        ViewBag.Categories = data;
        //        return PartialView("_Create_Edit", new CategoryEntity());
        //    }
        //}
        //public ActionResult Edit(int Id)
        //{
        //    List<CategoryEntity> list = _categoryService.GetAllWithLevel("");
        //    if (list == null)
        //    {
        //        ViewBag.Categories = new List<CategoryEntity>();
        //        return PartialView("_Create_Edit", new CategoryEntity());
        //    }
        //    else
        //    {
        //        list = list.Where(t => t.Id != Id).ToList();
        //        var data = getChildList(list, null, "--- ");
        //        ViewBag.Categories = data;
        //        CategoryEntity category = _categoryService.GetByID(Id);
        //        return PartialView("_Create_Edit", category);
        //    }
        //}
        public JsonResult Insert_Update(CategoryEntity category)
        {
            try
            {
                //if (category.Code != "")
                //{
                //    if (_categoryService.CheckExistCode(category.Id, category.Code))
                //    {
                //        return Json(new { warning = true, message = "Mã danh mục đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                //    }
                //}
                return Json(new { success = _categoryService.Insert_Update(category) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, message = ex }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
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