using Framework.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using LIB;
using WebApplication.Authorize;
using LIB.Product;
using LibCore.Helpers;
using System.Linq;
using System.Web;
using System.Configuration;
using static LIB.ExcelExtension;
using System.Threading.Tasks;
using LIB.ProductCate;
using WebApplication.Helpers;

namespace WebApplication.Controllers
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
        // GET: Products
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.Product }, ActionType = new ActionType[] { ActionType.View })]
        public ActionResult Index()
        {
            return View("Index");
        }
      
        public ActionResult Create()
        {           
            List<CategoryEntity> list = _categoryService.GetAllWithLevel("");
            var data = Tree.getChildList(list, null);

            List<SelectListItem> lst = data.Select(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();
            ViewBag.CategoryList = lst;
            return View("CreateOrEdit", new ProductEntity());
        }
        public ActionResult Edit(int? Id)
        {
            List<CategoryEntity> list = _categoryService.GetAllWithLevel("");
            var data = Tree.getChildList(list, null);
            var function = _productcateSevice.GetCateProuctId(Id.Value);
            List<SelectListItem> lst = data.Select(t => new SelectListItem()
            {
                Selected = function != null ? function.Where(f => f.CateId == t.Id).Count() > 0 ? true : false : false,
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();
            ViewBag.CategoryList = lst;
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
                        string message2 = "";
                        var ProductId = _productService.InsertOrUpdate(data, ref message).Id;
                        var listProductCate = _productcateSevice.GetCateProuctId(ProductId.Value);

                        if (listProductCate.Count > 0)
                        {
                            foreach (var item in listProductCate)
                            {
                                _productcateSevice.Delete(item);
                            }
                        }
                        //for (int i = 0; i < selectCate.Length; i++)
                        //{
                        //    var model = new ProductCateEntity
                        //    {
                        //        ProductId = int.Parse(ProductId.ToString()),
                        //        CateId = selectCate[i]
                        //    };
                        //    var productcate = _productcateSevice.Insert(model, ref message2);

                        //}
                        if (ProductId == null)
                        {
                            return Json(new { success = false, message }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = true, message, type = (ProductId == null ? 0 : 1) }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else // Update product
                {
                    if (_productService.CheckName(data.ProductName))
                    {
                        string message = "";
                        string message2 = "";
                        var ProductId = _productService.InsertOrUpdate(data, ref message).Id;
                        var listProductCate = _productcateSevice.GetCateProuctId(ProductId.Value);

                        if (listProductCate.Count > 0)
                        {
                            foreach (var item in listProductCate)
                            {
                                _productcateSevice.Delete(item);
                            }
                        }
                        //for (int i = 0; i < selectCate.Length; i++)
                        //{
                        //    var model = new ProductCateEntity
                        //    {
                        //        ProductId = int.Parse(ProductId.ToString()),
                        //        CateId = selectCate[i]

                        //    };
                        //    var productcate = _productcateSevice.Insert(model, ref message2);

                        //}
                        if (ProductId == null)
                        {
                            return Json(new { success = false, message }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = true, message, type = (ProductId == null ? 0 : 1) }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Sản phẩm này chưa tồn tại!" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult AutoCompletedProduct(Select2Param param, int? WarehouseId)
        {
            try
            {
                int totalRecord = 0;
                bootstrapTableParam obj = new bootstrapTableParam();
                obj.limit = 10;
                obj.offset = (param.page - 1) * 10;
                obj.search = param.term;
                List<ProductEntity> data = _productService.GetDataProductHaveQuantity(obj, WarehouseId ?? User.WarehouseId.Value, ref totalRecord);
                return Json(new { success = true, results = data, total = totalRecord }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.Product }, ActionType = new ActionType[] { ActionType.Record })]
        public JsonResult UpdateProduct(ProductEntity product, int? WarehouseId)
        {
            try
            {
                return Json(new { success = _productService.UpdateSellPrice(product.Id.Value, product.Price.Value, WarehouseId ?? User.WarehouseId.Value) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}