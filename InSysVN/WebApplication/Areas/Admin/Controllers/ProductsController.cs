using LIB.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LIB;
using LIB.ProductCate;
using Framework.EF;
using WebApplication.Authorize;
using System.Configuration;

namespace WebApplication.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProduct _productService;
        private readonly ICategory _categoryService;
        private readonly IProductCate _productcateSevice;
        public ProductsController()
        {
            _productService = SingletonIpl.GetInstance<IplProduct>();
            _categoryService = SingletonIpl.GetInstance<IplCategory>();
            _productcateSevice = SingletonIpl.GetInstance<IplProductCate>();
        }
        // GET: Admin/Products
        public ActionResult Index()
        {
            return View();
        }
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.Product }, ActionType = new ActionType[] { ActionType.View })]
        [HttpPost]
        public JsonResult GetDataProducts(bootstrapTableParam obj)
        {
            try
            {
                int totalRecord = 0;
                List<ProductEntity> datas = _productService.GetDataProduct(obj, ref totalRecord);
                datas.All(t => { t.AllowEdit = true; return true; });
                if (User.IsStaff)
                {
                    datas.All(t => { t.AllowEdit = false; return true; });
                }
                return Json(new { success = true, data = datas, total = totalRecord, AllowEdit = (User.IsStaff == true ? false : true) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return JsonResultError(ex);
            }
        }
        public ActionResult Create()
        {
            List<CategoryEntity> lstcate = _categoryService.GetAllData();
            List<SelectListItem> lst = lstcate.Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();
            ViewBag.ListCategory = lst;
            return View("CreateOrEdit", new ProductEntity());
        }
        public ActionResult Edit(int? Id)
        {
            List<CategoryEntity> lstcate = _categoryService.GetAllData();
            List<SelectListItem> lst = lstcate.Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();
            ViewBag.ListCategory = lst;
            var Product = _productService.GetByID(Id.Value);
            return View("CreateOrEdit", Product);
        }
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.Product }, ActionType = new ActionType[] { ActionType.View })]
        [HttpPost]
        public JsonResult InsertProduct(ProductEntity data, string base64Avatar, string fileName)
        {
            if (data.Id <= 0 || data.Id == null) // insert sản phẩm mới
            {
                if (_productService.CheckName(data.ProductName))
                {
                    return Json(new { success = false, message = "Sản phẩm đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string message = "";
                    ProductEntity ProductId = _productService.InsertOrUpdate(data, ref message);

                    if (ProductId != null)
                    {
                        if (fileName != null)
                        {
                            string PathServer = ControllerContext.HttpContext.Server.MapPath("~");
                            string PathFile = ConfigurationManager.AppSettings["PathUploadProductImage"] + fileName;
                            _productService.UpdateProductImage(base64Avatar, ProductId.Id.Value, PathServer, PathFile);
                        }
                        return Json(new { success = true, type = (ProductId.Id == null ? 0 : 1) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }                    
                }

            }
            else // Update product
            {
                if (_productService.CheckName(data.ProductName))
                {
                    string message = "";
                    ProductEntity ProductId = _productService.InsertOrUpdate(data, ref message);

                    if (ProductId != null)
                    {
                        if (fileName != null)
                        {
                            string PathServer = ControllerContext.HttpContext.Server.MapPath("~");
                            string PathFile = ConfigurationManager.AppSettings["PathUploadProductImage"] + fileName;
                            _productService.UpdateProductImage(base64Avatar, ProductId.Id.Value, PathServer, PathFile);
                        }
                        return Json(new { success = true, type = (ProductId.Id == null ? 0 : 1) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Sản phẩm này chưa tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public JsonResult UpdateIsActive(int productId)
        {
            bool flag = _productService.UpdateStatus(productId);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int Id)
        {
            try
            {
                return Json(new { success = _productService.DeletebyProductId(Id) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}