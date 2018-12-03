using System;
using System.Collections.Generic;

namespace LIB.Product
{
    public interface IProduct : IBaseServices<ProductEntity, long>
    {
        List<ProductEntity> GetDataProduct(bootstrapTableParam obj, ref int totalRecord);
        List<ProductEntity> GetDataProductHaveQuantity(bootstrapTableParam obj, int? WarehouseId, ref int totalRecord);
        List<ProductReport> GetProductReport(bootstrapTableParam obj, DateTime? startDate, DateTime? endDate, int? WarhouseId, ref int totalRecord);
        ProductEntity GetProductById_WarehouseId(long Id, int WarehouseID);
        //ResultMessageModel Products_SyncNew(int WarehouseId);
        bool UpdateSellPrice(long ProductId, decimal SellPrice, int WarehouseId);
        bool CheckByBarcode(string Barcode);
        ProductEntity GetByBarcode(string Barcode, int WarehouseID);
        ProductEntity GetByID(long ID);
        ProductEntity InsertOrUpdate(ProductEntity product, ref string message);
        List<ProductsChartTop10Model> GetProducts_ChartTop10(int opSearch, int IDWarehouse);
        int SaveSyncData(string xml);
    }
}
