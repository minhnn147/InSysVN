CREATE PROC [dbo].[sp_Products_GetAllProductSync]
@WarehouseId INT
as
SELECT
	Barcode
	,Allowcated
	,WarehouseId 
FROM dbo.Products
WHERE WarehouseId = @WarehouseId