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
        public JsonResult InsertProduct(ProductEntity data, int[] selectCate)
        {
                if (data.Id <= 0 || data.Id == null) // insert sản phẩm mới
                {
                    if (_productService.CheckByBarcode(data.Barcode))
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
                        for (int i = 0; i < selectCate.Length; i++)
                        {
                            var model = new ProductCateEntity
                            {
                                ProductId = int.Parse(ProductId.ToString()),
                                CateId = selectCate[i]

                            };
                            var productcate = _productcateSevice.Insert(model, ref message2);

                        }
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
                    if (_productService.CheckByBarcode(data.Barcode))
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
                        for (int i = 0; i < selectCate.Length; i++)
                        {
                            var model = new ProductCateEntity
                            {
                                ProductId = int.Parse(ProductId.ToString()),
                                CateId = selectCate[i]

                            };
                            var productcate = _productcateSevice.Insert(model, ref message2);

                        }
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

        [UserAuthorize(Modules = new ActionModule[] { ActionModule.Product }, ActionType = new ActionType[] { ActionType.Report })]
        public ActionResult Report()
        {
            return View("Report");
        }
        public ActionResult GetDataProductReport(bootstrapTableParam obj, DateTime? startDate, DateTime? endDate, int? WarehouseId)
        {
            try
            {
                int totalRecord = 0;
                var datas = _productService.GetProductReport(obj, startDate, endDate, User.WarehouseId ?? WarehouseId, ref totalRecord);
                return Json(new { success = true, data = datas, total = totalRecord }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ExportReport(string txtSearch, DateTime? startDate, DateTime? endDate, int? WarehouseId)
        {
            try
            {
                MemoryStream st = new MemoryStream();
                string UrlTemplate = ControllerContext.HttpContext.Server.MapPath(ConfigurationManager.AppSettings["Template_Export_ProductsReport"]);
                using (ExcelTemplateHelper helper = new ExcelTemplateHelper(UrlTemplate, st))
                {
                    helper.Direction = ExcelTemplateHelper.DirectionType.TOP_TO_DOWN;
                    helper.CurrentSheetName = "Sheet2";
                    helper.TempSheetName = "Sheet1";
                    helper.CurrentPosition = new CellPosition("A1");

                    int totalRecord = 0;
                    bootstrapTableParam obj = new bootstrapTableParam();
                    obj.limit = 0;
                    obj.search = txtSearch;
                    var datas = _productService.GetProductReport(obj, startDate, endDate, WarehouseId ?? User.WarehouseId, ref totalRecord);

                    var temp_head = helper.CreateTemplate("head");
                    var temp_body = helper.CreateTemplate("body");

                    helper.Insert(temp_head);
                    helper.InsertDatas(temp_body, datas);
                    helper.CopyWidth();
                }

                string fileName = "ProductsReport";
                if (startDate != null) fileName += "_" + startDate.Value.ToString("dd-MM-yyyy");
                if (endDate != null) fileName += "_" + endDate.Value.ToString("dd-MM-yyyy");
                string pathFile = ConfigurationManager.AppSettings["PathSaveFileExport"] + fileName + ".xlsx";
                FileStream fileStream = new FileStream(Server.MapPath(pathFile), FileMode.Create, FileAccess.Write);
                st.WriteTo(fileStream);
                fileStream.Close();
                return Json(new { success = true, urlFile = pathFile, fileName }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
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
        [UserAuthorize(Modules = new ActionModule[] { ActionModule.Product }, ActionType = new ActionType[] { ActionType.Export })]
        public JsonResult Export(string txtSearch, int? WarehouseId)
        {
            MemoryStream st = new MemoryStream();
            string UrlTemplate = ControllerContext.HttpContext.Server.MapPath(@"~/Templates/Export/ProductsExport.xlsx");
            using (ExcelTemplateHelper helper = new ExcelTemplateHelper(UrlTemplate, st))
            {
                helper.Direction = ExcelTemplateHelper.DirectionType.TOP_TO_DOWN;
                helper.CurrentSheetName = "Sheet2";
                helper.TempSheetName = "Sheet1";
                helper.CurrentPosition = new CellPosition("A1");

                int totalRecord = 0;
                bootstrapTableParam obj = new bootstrapTableParam();
                obj.limit = 0;
                obj.search = txtSearch;
                List<ProductEntity> datas = _productService.GetDataProduct(obj, ref totalRecord);

                var temp_head = helper.CreateTemplate("head");
                var temp_body = helper.CreateTemplate("body");

                helper.Insert(temp_head);
                helper.InsertDatas(temp_body, datas);
                helper.CopyWidth();
            }

            string fileName = "Products_";
            fileName += DateTime.Now.ToString("dd-MM-yyyy");

            FileStream fileStream = new FileStream(Server.MapPath(@"~/Download/Export/" + fileName + ".xlsx"), FileMode.Create, FileAccess.Write);
            st.WriteTo(fileStream);
            fileStream.Close();

            return Json(new { urlFile = "/Download/Export/" + fileName + ".xlsx", fileName }, JsonRequestBehavior.AllowGet);
        }

        //[UserAuthorize(Modules = new ActionModule[] { ActionModule.Product }, ActionType = new ActionType[] { ActionType.Import })]
        //public JsonResult UpdateImportProducts(List<ProductEntity> data, int? WarehouseId, List<string> listError)
        //{
        //    if (data.Count > 0)
        //    {
        //        data.ForEach(p =>
        //        {
        //            var productCheck = _productService.GetByBarcode(p.Barcode, WarehouseId ?? User.WarehouseId.Value);
        //            if (productCheck == null)
        //            {
        //                List<int?> CateId = new List<int?>();
        //                string status = "";
        //                string[] CateCode = System.Text.RegularExpressions.Regex.Split(p.ProductCategory, ",");
        //                foreach (var _catecode in CateCode)
        //                {
        //                    var cate = _categoryService.GetByCode(_catecode.Trim());
        //                    if (cate != null)
        //                    { CateId.Add(cate.Id); }
        //                }
        //                //p.ProductCategory = null;
        //                string message2 = "";
        //                ProductEntity product = _productService.InsertOrUpdate(p, ref status);
        //                foreach (var _CateId in CateId)
        //                {
        //                    var model = new ProductCateEntity
        //                    {
        //                        ProductId = int.Parse(product.Id.ToString()),
        //                        CateId = _CateId.Value

        //                    };
        //                    var productcate = _productcateSevice.Insert(model, ref message2);

        //                }
        //                if (product == null) p.ImportStatus = status;
        //                else p.ImportStatus = "1";
        //            }
        //            else
        //            {
        //                string status = "";
        //                List<int?> CateId = new List<int?>();
        //                string[] CateCode = System.Text.RegularExpressions.Regex.Split(p.ProductCategory, ",");
        //                foreach (var _catecode in CateCode)
        //                {
        //                    var cate = _categoryService.GetByCode(_catecode.Trim());
        //                    if (cate != null)
        //                    { CateId.Add(cate.Id); }
        //                }
        //                //p.ProductCategory = null;
        //                productCheck.InventoryNumber = productCheck.InventoryNumber + p.InventoryNumber;
        //                ProductEntity product = _productService.InsertOrUpdate(productCheck, ref status);
        //                string message2 = "";
        //                foreach (var _CateId in CateId)
        //                {
        //                    var model = new ProductCateEntity
        //                    {
        //                        ProductId = int.Parse(product.Id.ToString()),
        //                        CateId = _CateId.Value

        //                    };
        //                    var productcate = _productcateSevice.Insert(model, ref message2);

        //                }
        //                if (status == "")
        //                {
        //                    p.ImportStatus = "2";
        //                }
        //            }
        //        });
        //    }
        //    return Json(new { success = true, list = data, listError  }, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetChartTop10(int opSearch, int? IDWarehouse)
        {
            List<ProductsChartTop10Model> list = _productService.GetProducts_ChartTop10(opSearch, IDWarehouse ?? User.WarehouseId ?? 0);
            if (list != null)
            {
                List<string> listLabel = list.Select(t => t.Barcode).ToList();
                List<int> listQuantity = list.Select(t => t.Quantity).ToList();
                return Json(new { success = true, listLabel, listQuantity }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}