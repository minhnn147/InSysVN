CREATE PROC [dbo].[sp_Product_CheckByBarcode]
@Barcode NVARCHAR(50)
AS
BEGIN
	SELECT COUNT(*) FROM dbo.Products WHERE Barcode = @Barcode
END