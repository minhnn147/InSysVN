CREATE PROC [dbo].[sp_Input_Return_GetById]
@Id BIGINT
AS
SELECT TOP 1 ir.*, p.ProductName, p.Barcode, p.SellPrice
FROM dbo.Input_Return ir
JOIN dbo.Products p ON ir.ProductId = p.Id
WHERE ir.Id = @Id AND ir.isActive = 1