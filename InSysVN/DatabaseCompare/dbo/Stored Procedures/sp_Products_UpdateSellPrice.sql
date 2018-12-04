CREATE PROC [dbo].[sp_Products_UpdateSellPrice]
@ProductId BIGINT,
@SellPrice DECIMAL,
@WarehouseId INT 
AS
UPDATE dbo.Products SET SellPriceShop = @SellPrice, ModifiedDate = GETDATE()
WHERE Id = @ProductId AND WarehouseId = @WarehouseId