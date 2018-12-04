CREATE PROC sp_Products_GetByBarcode
@Barcode NVARCHAR(50),
@WarehouseId INT
as
SELECT Id ,
       ProductCategory ,
       ProductName ,
       ProductCode ,
       Barcode ,
       MainImage ,
       Image ,
       Description ,
       Status ,
       CreatedDate ,
       ModifiedDate ,
       InventoryNumber ,
       ComputeUnit ,
       Price ,
       SellPrice ,
       ExpiredDate ,
       Allowcated ,
       WarehouseId ,
       DateSync FROM dbo.Products WHERE Barcode = @Barcode AND WarehouseId = @WarehouseId