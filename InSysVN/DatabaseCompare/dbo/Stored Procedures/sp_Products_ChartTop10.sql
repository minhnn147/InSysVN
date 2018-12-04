
CREATE PROC [dbo].[sp_Products_ChartTop10]
@opSearch INT,
@IDWarehouse INT
AS

DECLARE @qr NVARCHAR(MAX),
		@qrSellWhere NVARCHAR(MAX) ='',
		@qrReturnWhere NVARCHAR(MAX) = '',
		@qrWhere NVarchar(max) = '';
--hôm nay
IF @opSearch = 1
BEGIN
	SET @qrSellWhere = ' AND CONVERT(DATE,o.OrderDate) = CONVERT(DATE,GETDATE()) ';
	SET @qrReturnWhere = ' AND  CONVERT(DATE,CASE WHEN r.ModifiedDate IS NULL THEN r.CreatedDate ELSE r.ModifiedDate END) = CONVERT(DATE,GETDATE()) ';
END
--tuần này
ELSE IF @opSearch = 2
BEGIN
	SET @qrSellWhere = ' AND DATEPART(wk,o.OrderDate) = DATEPART(wk,GETDATE()) ';
	SET @qrReturnWhere = ' AND  DATEPART(wk,CASE WHEN r.ModifiedDate IS NULL THEN r.CreatedDate ELSE r.ModifiedDate END) = DATEPART(wk,GETDATE()) ';
END
--tháng này
ELSE IF @opSearch = 3
BEGIN
	SET @qrSellWhere = ' AND CONVERT(VARCHAR(7),o.OrderDate,126) = CONVERT(VARCHAR(7), GETDATE(), 126) ';
	SET @qrReturnWhere = ' AND  CONVERT(VARCHAR(7),CASE WHEN r.ModifiedDate IS NULL THEN r.CreatedDate ELSE r.ModifiedDate END,126) = CONVERT(VARCHAR(7), GETDATE(), 126) ';
END
--năm nay
ELSE IF @opSearch = 4
BEGIN
	SET @qrSellWhere = ' AND YEAR(o.OrderDate) = YEAR(GETDATE()) ';
	SET @qrReturnWhere = ' AND  YEAR(CASE WHEN r.ModifiedDate IS NULL THEN r.CreatedDate ELSE r.ModifiedDate END) = YEAR(GETDATE()) ';
END

if @IDWarehouse <> 0
begin
 SET @qrWhere = ' AND w.Id = @IDWarehouse ' ;
end

SET @qr = '
			SELECT	TOP 10
					sell.Barcode
					,Quantity = (sell.QuantitySell - ISNULL(ret.QuantityReturn,0))
			FROM
			(
				SELECT	od.Barcode
						,QuantitySell = SUM(od.Quantity)
				FROM dbo.OrderDetail od
				JOIN dbo.Orders o ON od.OrderId = o.Id
				JOIN dbo.Warehouses w ON o.WarehouseId = w.Id
				WHERE o.isActive = 1 AND o.Status = 1 AND w.isActive = 1 '+@qrSellWhere+@qrWhere+'
				GROUP BY od.Barcode
			) AS sell
			LEFT JOIN
			(
				SELECT	p.Barcode
						,QuantityReturn = SUM(rd.QuantityReturn)
				FROM dbo.ReturnDetail rd
				JOIN dbo.[Return] r ON rd.ReturnId = r.Id
				JOIN dbo.Orders o ON r.OrderId = o.Id
				JOIN dbo.Warehouses w ON o.WarehouseId = w.Id
				JOIN dbo.Products p ON rd.ProductId = p.Id
				WHERE o.isActive = 1 AND o.Status = 1 AND w.isActive = 1 '+@qrReturnWhere+@qrWhere+'
				GROUP BY p.Barcode
			) AS ret ON sell.Barcode = ret.Barcode
			ORDER BY Quantity DESC
';
PRINT @qr
EXECUTE sp_executesql @qr,N'@IDWarehouse INT',@IDWarehouse;