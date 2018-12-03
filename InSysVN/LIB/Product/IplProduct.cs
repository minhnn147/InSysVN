using Dapper;
using Framework.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LIB.Product
{
    public class IplProduct : BaseService<ProductEntity, long>, IProduct
    {
        public List<ProductEntity> GetDataProduct(bootstrapTableParam obj, ref int totalRecord)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@txtSearch", obj.search);
                param.Add("@pageNumber", obj.pageNumber());
                param.Add("@pageSize", obj.pageSize());
                param.Add("@order", obj.order);
                param.Add("@sort", obj.sort);
                param.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var data = unitOfWork.Procedure<ProductEntity>("sp_Products_GetData", param).ToList();
                totalRecord = param.Get<int>("@totalRecord");
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public List<ProductEntity> GetDataProductHaveQuantity(bootstrapTableParam obj, int? WarehouseId, ref int totalRecord)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@txtSearch", obj.search);
                param.Add("@WarehouseId", WarehouseId);
                param.Add("@pageNumber", obj.pageNumber());
                param.Add("@pageSize", obj.pageSize());
                param.Add("@order", obj.order);
                param.Add("@sort", obj.sort);
                param.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var data = unitOfWork.Procedure<ProductEntity>("sp_Products_GetDataHaveQuantity", param).ToList();
                totalRecord = param.Get<int>("@totalRecord");
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public List<ProductReport> GetProductReport(bootstrapTableParam obj, DateTime? startDate, DateTime? endDate, int? WarhouseId, ref int totalRecord)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@txtSearch", obj.search);
                param.Add("@startDate", startDate);
                param.Add("@endDate", endDate);
                param.Add("@WarehouseId", WarhouseId);
                param.Add("@pageNumber", obj.pageNumber());
                param.Add("@pageSize", obj.pageSize());
                param.Add("@order", obj.order);
                param.Add("@sort", obj.sort);
                param.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var data = unitOfWork.Procedure<ProductReport>("sp_Products_Report", param).ToList();
                totalRecord = param.Get<int>("@totalRecord");
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public ProductEntity GetProductById_WarehouseId(long Id, int WarehouseID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WarehouseID", WarehouseID);
                param.Add("@Id", Id);
                return unitOfWork.Procedure<ProductEntity>("sp_Product_GetProductById_WarehouseId", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        //public ResultMessageModel Products_SyncNew(int WarehouseId)
        //{
        //    _APIService = SingletonIpl.GetInstance<IplAPI>();
        //    return _APIService.API_Warehouses_SyncNewProduct(WarehouseId).Result;
        //}
        public bool UpdateSellPrice(long ProductId, decimal SellPriceShop, int WarehouseId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductId", ProductId);
                param.Add("@SellPrice", SellPriceShop);
                param.Add("@WarehouseId", WarehouseId);
                return unitOfWork.ProcedureExecute("sp_Products_UpdateSellPrice", param);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
        public ProductEntity GetByID(long ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", ID);
                return unitOfWork.Procedure<ProductEntity>("sp_Products_GetById", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public bool CheckByBarcode(string Barcode)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Barcode", Barcode);
                if (unitOfWork.Procedure<int>("sp_Product_CheckByBarcode", param).SingleOrDefault() > 0)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

        }
        public ProductEntity GetByBarcode(string Barcode, int WarehouseID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Barcode", Barcode);
                param.Add("@WarehouseId", WarehouseID);
                ProductEntity product = unitOfWork.Procedure<ProductEntity>("sp_Products_GetByBarcode", param).SingleOrDefault();
                return product;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }

        }
        public ProductEntity InsertOrUpdate(ProductEntity product, ref string message)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", product.Id);
                param.Add("@ProductCategory", product.ProductCategory);
                param.Add("@ProductName", product.ProductName);
                param.Add("@Barcode", product.Barcode);
                param.Add("@InventoryNumber", product.Quantity);
                param.Add("@ComputeUnit", product.ComputeUnit);
                param.Add("@Price", product.Price);
                param.Add("@SellPrice", product.Price);
                param.Add("@Description", product.Description);
                return unitOfWork.Procedure<ProductEntity>("sp_Products_InsertOrUpdate", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public List<ProductsChartTop10Model> GetProducts_ChartTop10(int opSearch,int IDWarehouse)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@opSearch", opSearch);
                param.Add("@IDWarehouse", IDWarehouse);
                return unitOfWork.Procedure<ProductsChartTop10Model>("sp_Products_ChartTop10", param).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public int SaveSyncData(string xml)
        {
            try
            {
                const string procedure = "Jod_Products_SaveSyncData";
                var param = new DynamicParameters();
                param.Add("@xml", xml);
                var result = unitOfWork.Procedure<int>(procedure, param).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
